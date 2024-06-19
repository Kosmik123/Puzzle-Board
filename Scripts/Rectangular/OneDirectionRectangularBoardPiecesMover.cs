using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
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

        private ScenePiece CreateScenePiece(Piece piece)
        {
            var pieceComponent = PiecesSpawner.SpawnPiece(piece);
            return pieceComponent;
        }

        public override void HandleCollapseMovemement(OneDirectionRectangularBoardCollapseStrategy strategy, IReadOnlyList<ICollapseEventArgs> collapseEvents)
        {
            IsMoving = true;
            piecesMovementManager.OnAllPiecesMovementStopped += PiecesMovementManager_OnAllPiecesMovementStopped;
            for (int i = 0; i < collapseEvents.Count; i++)
            {
                var collapseEventArgs = collapseEvents[i];
                if (collapseEventArgs is OneDirectionRectangularBoardCollapseStrategy.PieceCollapsedEventArgs collapseEvent)
                {
                    var piece = collapseEvent.Piece;
                    var scenePiece = SceneBoard.GetScenePiece(piece);
                    if (scenePiece)
                    {
                        scenePiece.Coord = collapseEvent.TargetCoord;
                        piecesMovementManager.StartPieceMovement(scenePiece, collapseEvent.TargetCoord);
                    }
                }
                else if (dontRefillEmptySpaces == false && collapseEventArgs is IPieceCreatedCollapseEventArgs createEvent)
                {
                    var piece = createEvent.Piece;
                    var scenePiece = CreateScenePiece(piece);
                    scenePiece.Coord = createEvent.CreationCoord;
                    var collapseDirection = BoardHelper.GetCorrectedDirection(createEvent.CreationCoord, strategy.CollapseDirection, SceneBoard.Board.Layout == GridLayout.CellLayout.Hexagon);
                    var spawnCoord = createEvent.CreationCoord;
                    {
                        spawnCoord[strategy.CollapseAxis] = collapseDirection[strategy.CollapseAxis] switch
                        {
                            1 => -1,
                            -1 => SceneBoard.GetBoard().Dimensions[strategy.CollapseAxis],
                            _ => 0
                        };
                    }

                    spawnCoord -= collapseDirection * createEvent.CreateIndex;
                    scenePiece.transform.position = SceneBoard.CoordToWorld(spawnCoord);

                    piecesMovementManager.StartPieceMovement(scenePiece, createEvent.CreationCoord);
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
