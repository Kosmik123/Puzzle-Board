using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public class OneDirectionRectangularBoardPiecesMover : PiecesMover<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    {
        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

        [SerializeField]
        private DefaultPiecesMovementManager piecesMovementManager;

        [SerializeField]
        private bool dontRefillEmptySpaces;

        private PieceComponent CreatePiece(Piece piece)
        {
            var pieceComponent = PiecesSpawner.SpawnPiece(piece);
            return pieceComponent;
        }

        public override void HandleCollapseMovemement(OneDirectionRectangularBoardCollapseStrategy strategy, IReadOnlyList<ICollapseEventArgs> collapseEvents)
        {
            for (int i = 0; i < collapseEvents.Count; i++)
            {
                var collapseEventArgs = collapseEvents[i];
                if (collapseEventArgs is OneDirectionRectangularBoardCollapseStrategy.PieceCollapsedEventArgs collapseEvent)
                {
                    var piece = collapseEvent.Piece;
                    var pieceComponent = BoardComponent.GetPieceComponent(piece);
                    piecesMovementManager.StartPieceMovement(pieceComponent, piece.Coord);
                }
                else if (dontRefillEmptySpaces == false && collapseEventArgs is IPieceCreatedCollapseEventArgs createEvent)
                {
                    var piece = createEvent.Piece;
                    var pieceComponent = CreatePiece(piece);
                    var collapseDirection = BoardHelper.GetCorrectedDirection(piece.Coord, strategy.CollapseDirection, BoardComponent.Board.Layout == GridLayout.CellLayout.Hexagon);

                    var spawnCoord = piece.Coord;
                    {
                        spawnCoord[strategy.CollapseAxis] = collapseDirection[strategy.CollapseAxis] switch
                        {
                            1 => -1,
                            -1 => BoardComponent.GetBoard().Dimensions[strategy.CollapseAxis],
                            _ => 0
                        };
                    }

                    spawnCoord -= collapseDirection * createEvent.CreateIndex;
                    pieceComponent.transform.position = BoardComponent.CoordToWorld(spawnCoord);
                    piecesMovementManager.StartPieceMovement(pieceComponent, piece.Coord);
                }
            }
        }

        private void PiecesMovementManager_OnAllPiecesMovementStopped()
        {
            piecesMovementManager.OnAllPiecesMovementStopped -= PiecesMovementManager_OnAllPiecesMovementStopped;
            IsMoving = false;
        }
    }
}
