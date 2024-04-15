using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    [RequireComponent(typeof(RectangularBoardComponent))] // we are slowly deprecating this
    public /* no more */ abstract  class OneDirectionRectangularBoardCollapsing : BoardCollapseController<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    {
        public override event System.Action OnPiecesColapsed;

        [SerializeField]
        protected DefaultPiecesMovementManager piecesMovementManager;

        [SerializeField, CollapseDirection]
        private Vector2Int collapseDirection;
        public Vector2Int RealCollapseDirection
        {
            get
            {
                return Vector2Int.RoundToInt(Grid.Swizzle(BoardComponent.Grid.cellSwizzle, (Vector2)collapseDirection));
            }
        }
        private int iterationAxis;
        private int collapseAxis;

        public override bool IsCollapsing => piecesMovementManager.ArePiecesMoving;

        protected virtual void Reset()
        {
            piecesMovementManager = FindObjectOfType<DefaultPiecesMovementManager>();
        }

        protected virtual void Awake()
        {
            CalculateAxes();
        }

        public override void Collapse()
        {
            //bool colapsed = false;
            //for (int lineIndex = 0; lineIndex < BoardComponent.Dimensions[iterationAxis]; lineIndex++)
            //{
            //    int emptyCellsCount = CollapseTokensInLine(lineIndex);
            //    if (emptyCellsCount > 0)
            //    {
            //        colapsed = true;
            //        RefillLine(lineIndex, emptyCellsCount);
            //    }
            //}

            //if (colapsed)
            //    piecesMovementManager.OnAllPiecesMovementStopped += CallCollapseEvent;
        }

        private void IterateOverCellsInLine(int lineIndex, int count, int startCellIndex, int iterationDirection, System.Action<Vector2Int> action)
        {
            //int lineSize = BoardComponent.Dimensions[collapseAxis];
            //for (int i = 0; i < count; i++)
            //{
            //    var coord = Vector2Int.zero;
            //    coord[iterationAxis] = lineIndex;
            //    coord[collapseAxis] = (startCellIndex + i * iterationDirection + lineSize) % lineSize;

            //    action.Invoke(coord);
            //}
        }

        private int CollapseTokensInLine(int lineIndex)
        {
            int lineSize = 1; // BoardComponent.Dimensions[collapseAxis];

            int startCellIndex = RealCollapseDirection[collapseAxis] > 0 ? -1 : 0; 
            int lineCollapseDirection = RealCollapseDirection[collapseAxis] == 0 ? 1 : -RealCollapseDirection[collapseAxis];

            int nonExistingPiecesCount = 0;


            IterateOverCellsInLine(lineIndex, lineSize, startCellIndex, lineCollapseDirection, (coord) =>
            {
                var piece = BoardComponent.GetPiece(coord);
                if (piece == null || piece.IsCleared)
                {
                    nonExistingPiecesCount++;
                }
                else if (nonExistingPiecesCount > 0)
                {
                    var offsetToMove = RealCollapseDirection * nonExistingPiecesCount;
                    var targetCoord = coord + offsetToMove;
                    BoardComponent.Board[coord] = null;
                    BoardComponent.Board[targetCoord] = piece.Piece;
                    piecesMovementManager.StartPieceMovement(piece, targetCoord, 0.3f);
                }
            });

            return nonExistingPiecesCount;
        }

        private void CallCollapseEvent()
        {
            piecesMovementManager.OnAllPiecesMovementStopped -= CallCollapseEvent;
            OnPiecesColapsed?.Invoke();
        }

        private void RefillLine(int lineIndex, int count)
        {
            int lineSize = 1;// BoardComponent.Dimensions[collapseAxis];

            int startCellIndex = RealCollapseDirection[collapseAxis] < 0 ? -1 : 0;
            var spawnOffset = -RealCollapseDirection * count;

            int refillingDirection = RealCollapseDirection[collapseAxis] == 0 ? 1 : RealCollapseDirection[collapseAxis];

            IterateOverCellsInLine(lineIndex, count, startCellIndex, refillingDirection, (coord) =>
            {
                var newPiece = CreatePiece(coord);
                var spawnCoord = coord + spawnOffset;
                newPiece.transform.position = BoardComponent.CoordToWorld(spawnCoord);
                piecesMovementManager.StartPieceMovement(newPiece, coord, 0.3f);
            });
        }

        private void OnValidate()
        {
            CalculateAxes();
        }

        private void CalculateAxes()
        {
            iterationAxis = (RealCollapseDirection.x != 0) ? 1 : 0;
            collapseAxis = 1 - iterationAxis;
        }
    }
}
