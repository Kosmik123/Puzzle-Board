using Bipolar.PuzzleBoard.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class LinearGeneralBoardPiecesMover : PiecesMover<LinearGeneralBoardCollapseStrategy, GeneralBoard>
    {
        [SerializeField]
        private PiecesSpawner piecesSpawner;
        public PiecesSpawner PiecesSpawner
        {
            get => piecesSpawner;
            set => piecesSpawner = value;
        }

        [SerializeField]
        private LinearGeneralBoardPiecesMovementManager piecesMovementManager;

        public override void HandleCollapseMovemement(LinearGeneralBoardCollapseStrategy strategy, IReadOnlyList<ICollapseEventArgs> collapseEvents)
        {
            IsMoving = true;
            piecesMovementManager.OnAllPiecesMovementStopped += PiecesMovementManager_OnAllPiecesMovementStopped;
            for (int i = 0; i < collapseEvents.Count; i++)
            {
                var collapseEventArgs = collapseEvents[i];
                if (collapseEventArgs is LinearGeneralBoardCollapseStrategy.PieceCollapsedEventArgs collapseEvent)
                {
                    var piece = collapseEvent.Piece;
                    var pieceComponent = BoardComponent.GetPieceComponent(piece);
                    piecesMovementManager.StartPieceMovement(pieceComponent, collapseEvent.Line, collapseEvent.FromIndex, collapseEvent.TargetCoord);
                }
                else if (collapseEventArgs is LinearGeneralBoardCollapseStrategy.PieceCreatedEventArgs createEvent)
                {
                    var piece = createEvent.Piece;
                    var pieceComponent = PiecesSpawner.SpawnPiece(piece);

                    var lineStartCoord = createEvent.Line.Coords[0];
                    var creatingDirection = -BoardHelper.GetCorrectedDirection(lineStartCoord, strategy.Directions[lineStartCoord], BoardComponent.Layout == GridLayout.CellLayout.Hexagon);
                    var firstCellPosition = BoardComponent.CoordToWorld(lineStartCoord);
                    var spawningPosition = firstCellPosition + (Vector3)((Vector2)creatingDirection * (createEvent.CreateIndex + 1));
                    pieceComponent.transform.position = spawningPosition;
                    piecesMovementManager.StartPieceMovement(pieceComponent, createEvent.Line, -1, createEvent.CreationCoord);
                }
            }
        }

        private void PiecesMovementManager_OnAllPiecesMovementStopped()
        {
            piecesMovementManager.OnAllPiecesMovementStopped -= PiecesMovementManager_OnAllPiecesMovementStopped;
            IsMoving = false;
        }

        public Vector2 GetRealDirection(Vector2Int coord, Vector2Int direction)
        {
            var nextCoord = coord + direction;

            var worldPos = BoardComponent.CoordToWorld(coord);
            var nextWorldPos = BoardComponent.CoordToWorld(nextCoord);

            return nextWorldPos - worldPos;
        }
    }
}
