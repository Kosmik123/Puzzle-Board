using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    [RequireComponent(typeof(RectangularBoard))]
    public class OneDirectionRectangularBoardCollapsing : BoardCollapsing<RectangularBoard>
    {
        public override event System.Action OnPiecesColapsed;

        protected DefaultPiecesMovementManager piecesMovementManager;

        [SerializeField, CollapseDirection]
        private Vector2Int collapseDirection;
        public Vector2Int CollapseDirection
        {
            get
            {
                return Vector2Int.RoundToInt(Grid.Swizzle(Board.Grid.cellSwizzle, (Vector2)collapseDirection));
            }
        }
        private int iterationAxis;
        public int collapseAxis;

        public override bool IsCollapsing => piecesMovementManager.ArePiecesMoving;

        protected virtual void Awake()
        {
            CalculateAxes();
        }

        public void Init(DefaultPiecesMovementManager piecesMovementManager)
        {
            this.piecesMovementManager = piecesMovementManager;
        }

        public override void Collapse()
        {
            bool colapsed = false;
            for (int lineIndex = 0; lineIndex < Board.Dimensions[iterationAxis]; lineIndex++)
            {
                int emptyCellsCount = CollapseTokensInLine(lineIndex);
                if (emptyCellsCount > 0)
                {
                    colapsed = true;
                    RefillLine(lineIndex, emptyCellsCount);
                }
            }

            if (colapsed)
                piecesMovementManager.OnAllPiecesMovementStopped += CallCollapseEvent;
        }

        private void IterateOverCellsInLine(int lineIndex, int count, int startCellIndex, int iterationDirection, System.Action<Vector2Int> action)
        {
            int lineSize = Board.Dimensions[collapseAxis];
            for (int i = 0; i < count; i++)
            {
                var coord = Vector2Int.zero;
                coord[iterationAxis] = lineIndex;
                coord[collapseAxis] = (startCellIndex + i * iterationDirection + lineSize) % lineSize;

                action.Invoke(coord);
            }
        }

        private int CollapseTokensInLine(int lineIndex)
        {
            int lineSize = Board.Dimensions[collapseAxis]; // to samo

            int startCellIndex = CollapseDirection[collapseAxis] > 0 ? -1 : 0; // odwrócony warunek
            int lineCollapseDirection = CollapseDirection[collapseAxis] == 0 ? 1 : -CollapseDirection[collapseAxis];

            int nonExistingPiecesCount = 0;


            IterateOverCellsInLine(lineIndex, lineSize, startCellIndex, lineCollapseDirection, (coord) =>
            {
                var piece = Board.GetPiece(coord);
                if (piece == null || piece.IsCleared)
                {
                    nonExistingPiecesCount++;
                }
                else if (nonExistingPiecesCount > 0)
                {
                    var offsetToMove = CollapseDirection * nonExistingPiecesCount;
                    var targetCoord = coord + offsetToMove;
                    Board[coord] = null;
                    Board[targetCoord] = piece;
                    piecesMovementManager.StartPieceMovement(piece, targetCoord, 0.3f);
                }
            });

            //for (int i = 0; i < lineSize; i++)
            //{
            //    var coord = Vector2Int.zero;
            //    coord[iterationAxis] = lineIndex;
            //    coord[collapseAxis] = (startCellIndex + i * lineCollapseDirection + lineSize) % lineSize;

            //    var piece = Board.GetPiece(coord);
            //    if (piece == null || piece.IsCleared)
            //    {
            //        nonExistingPiecesCount++;
            //    }
            //    else if (nonExistingPiecesCount > 0)
            //    {
            //        var offsetToMove = CollapseDirection * nonExistingPiecesCount;
            //        var targetCoord = coord + offsetToMove;
            //        Board[coord] = null;
            //        Board[targetCoord] = piece;
            //        piecesMovementManager.StartPieceMovement(piece, targetCoord, 0.3f);
            //    }
            //}

            return nonExistingPiecesCount;
        }

        private void CallCollapseEvent()
        {
            piecesMovementManager.OnAllPiecesMovementStopped -= CallCollapseEvent;
            OnPiecesColapsed?.Invoke();
        }

        private void RefillLine(int lineIndex, int count)
        {
            int lineSize = Board.Dimensions[collapseAxis];

            int startCellIndex = CollapseDirection[collapseAxis] < 0 ? -1 : 0;
            var spawnOffset = -CollapseDirection * count;

            int refillingDirection = CollapseDirection[collapseAxis] == 0 ? 1 : CollapseDirection[collapseAxis];

            IterateOverCellsInLine(lineIndex, count, startCellIndex, refillingDirection, (coord) =>
            {
                var newPiece = CreatePiece(coord);
                var spawnCoord = coord + spawnOffset;
                newPiece.transform.position = Board.CoordToWorld(spawnCoord);
                piecesMovementManager.StartPieceMovement(newPiece, coord, 0.3f);
            });

            //for (int i = 0; i < count; i++)
            //{
            //    var coord = Vector2Int.zero;
            //    coord[iterationAxis] = lineIndex;
            //    coord[collapseAxis] = (startCellIndex + i * refillingDirection + lineSize) % lineSize;

            //    var newPiece = CreatePiece(coord);
            //    var spawnCoord = coord + spawnOffset;
            //    newPiece.transform.position = Board.CoordToWorld(spawnCoord);
            //    piecesMovementManager.StartPieceMovement(newPiece, coord, 0.3f);
            //}
        }

        private void OnValidate()
        {
            CalculateAxes();
        }

        private void CalculateAxes()
        {
            iterationAxis = (CollapseDirection.x != 0) ? 1 : 0;
            collapseAxis = 1 - iterationAxis;
        }
    }
}
