using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class OneDirectionRectangularBoardCollapseStrategy : BoardCollapseStrategy<RectangularBoard>
    {
        [SerializeField, CollapseDirection]
        private Vector2Int collapseDirection;
        public Vector2Int CollapseDirection => collapseDirection;

        private int iterationAxis;
        private int collapseAxis;

        public override bool Collapse(RectangularBoard board, IPieceFactory pieceFactory)
        {
            CalculateAxes();

            bool colapsed = false;
            for (int lineIndex = 0; lineIndex < board.Dimensions[iterationAxis]; lineIndex++)
            {
                int emptyCellsCount = CollapseTokensInLine(lineIndex, board);
                if (emptyCellsCount > 0)
                {
                    colapsed = true;
                    RefillLine(lineIndex, emptyCellsCount, board, pieceFactory);
                }
            }
            return colapsed;
        }

        private int CollapseTokensInLine(int lineIndex, RectangularBoard board)
        {
            int lineSize = board.Dimensions[collapseAxis];

            int startCellIndex = CollapseDirection[collapseAxis] > 0 ? -1 : 0;
            int lineCollapseDirection = CollapseDirection[collapseAxis] == 0 ? 1 : -CollapseDirection[collapseAxis];

            int nonExistingPiecesCount = 0;


            IterateOverCellsInLine(board, lineIndex, lineSize, startCellIndex, lineCollapseDirection, (coord) =>
            {
                var piece = board[coord];
                if (piece == null || piece.IsCleared)
                {
                    nonExistingPiecesCount++;
                }
                else if (nonExistingPiecesCount > 0)
                {
                    var offsetToMove = CollapseDirection * nonExistingPiecesCount;
                    var targetCoord = coord + offsetToMove;
                    board[coord] = null;
                    board[targetCoord] = piece;
                }
            });

            return nonExistingPiecesCount;
        }

        private void RefillLine(int lineIndex, int count, RectangularBoard board, IPieceFactory pieceFactory)
        {
            int startCellIndex = CollapseDirection[collapseAxis] < 0 ? -1 : 0;
            var spawnOffset = -CollapseDirection * count;

            int refillingDirection = CollapseDirection[collapseAxis] == 0 ? 1 : CollapseDirection[collapseAxis];

            IterateOverCellsInLine(board, lineIndex, count, startCellIndex, refillingDirection, (coord) =>
            {
                board[coord] = (pieceFactory?.CreatePiece(coord.x, coord.y));
            });
        }

        private void IterateOverCellsInLine(RectangularBoard board, int lineIndex, int count, int startCellIndex, int iterationDirection, System.Action<Vector2Int> action)
        {
            int lineSize = board.Dimensions[collapseAxis];
            for (int i = 0; i < count; i++)
            {
                var coord = Vector2Int.zero;
                coord[iterationAxis] = lineIndex;
                coord[collapseAxis] = (startCellIndex + i * iterationDirection + lineSize) % lineSize;

                action.Invoke(coord);
            }
        }

        private void CalculateAxes()
        {
            iterationAxis = (CollapseDirection.x != 0) ? 1 : 0;
            collapseAxis = 1 - iterationAxis;
        }
    }
}
