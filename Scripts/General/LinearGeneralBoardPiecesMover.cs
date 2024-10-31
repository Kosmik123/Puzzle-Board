using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
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
                var eventArgs = collapseEvents[i];
                if (eventArgs is LinearGeneralBoardCollapseStrategy.PieceCollapsedEventArgs collapseEvent)
                {
                    var piece = collapseEvent.Piece;
                    var scenePiece = SceneBoard.GetScenePiece(piece);
                    piecesMovementManager.StartPieceMovement(scenePiece, collapseEvent.Line, collapseEvent.FromIndex, collapseEvent.TargetCoord);
                }
                else if (eventArgs is LinearGeneralBoardCollapseStrategy.PieceCreatedEventArgs createEvent)
                {
                    var piece = createEvent.Piece;
                    var scenePiece = PiecesSpawner.SpawnPiece(piece);

                    var lineStartCoord = createEvent.Line.Coords[0];
                    var creatingDirection = -BoardHelper.GetCorrectedDirection(lineStartCoord, strategy.Directions[lineStartCoord], SceneBoard.Layout == GridLayout.CellLayout.Hexagon);
                    var firstCellPosition = SceneBoard.CoordToWorld(lineStartCoord);
                    var spawningPosition = firstCellPosition + (Vector3)((Vector2)creatingDirection * (createEvent.CreateIndex + 1));
                    scenePiece.transform.position = spawningPosition;
                    piecesMovementManager.StartPieceMovement(scenePiece, createEvent.Line, -1, createEvent.CreationCoord);
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

            var worldPos = SceneBoard.CoordToWorld(coord);
            var nextWorldPos = SceneBoard.CoordToWorld(nextCoord);

            return nextWorldPos - worldPos;
        }
    }
}
