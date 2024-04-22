using UnityEngine;

namespace Bipolar.PuzzleBoard.Core
{
    [System.Serializable]
    public class OneDirectionRectangularBoardCollapseStrategy : OneDirectionCollapseStrategy<RectangularBoard>
    {
        public override event CollapseEventHandler OnPieceCollapsed;

        public override bool Collapse(RectangularBoard board, IPieceFactory pieceFactory)
        {
            RecalculateAxes();

            bool colapsed = false;
            for (int lineIndex = 0; lineIndex < board.Dimensions[iterationAxis]; lineIndex++)
            {
                int emptyCellsCount = CollapsePiecesInLine(lineIndex, board);
                if (emptyCellsCount > 0)
                {
                    colapsed = true;
                    RefillLine(lineIndex, emptyCellsCount, board, pieceFactory);
                }
            }
            return colapsed;
        }

        private int CollapsePiecesInLine(int lineIndex, RectangularBoard board)
        {
            int lineSize = board.Dimensions[CollapseAxis];

            int startCellIndex = CollapseDirection[CollapseAxis] > 0 ? -1 : 0;
            int lineCollapseDirection = CollapseDirection[CollapseAxis] == 0 ? 1 : -CollapseDirection[CollapseAxis];

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
                    piece.Coord = targetCoord;
                    OnPieceCollapsed?.Invoke(this, new PieceCollapsedEventArgs(piece, coord));
                }
            });

            return nonExistingPiecesCount;
        }

        private void RefillLine(int lineIndex, int count, RectangularBoard board, IPieceFactory pieceFactory)
        {
            int startCellIndex = CollapseDirection[CollapseAxis] < 0 ? -1 : 0;
            var spawnOffset = -CollapseDirection * count;

            int refillingDirection = CollapseDirection[CollapseAxis] == 0 ? 1 : CollapseDirection[CollapseAxis];

            int indexInLine = 0;
            IterateOverCellsInLine(board, lineIndex, count, startCellIndex, refillingDirection, (coord) =>
            {
                var piece = pieceFactory?.CreatePiece(coord.x, coord.y);
                board[coord] = piece;
                OnPieceCollapsed?.Invoke(this, new PieceCreatedEventArgs(piece, count - 1 - indexInLine, coord));
                indexInLine++;
            });
        }

        private void IterateOverCellsInLine(RectangularBoard board, int lineIndex, int count, int startCellIndex, int iterationDirection, System.Action<Vector2Int> action)
        {
            int lineSize = board.Dimensions[CollapseAxis];
            for (int i = 0; i < count; i++)
            {
                Vector2Int coord = default;
                coord[iterationAxis] = lineIndex;
                coord[CollapseAxis] = (startCellIndex + i * iterationDirection + lineSize) % lineSize;

                action.Invoke(coord);
            }
        }

    }
}
