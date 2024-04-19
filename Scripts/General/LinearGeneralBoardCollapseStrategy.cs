using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.General
{
    public class LinearGeneralBoardCollapseStrategy : BoardCollapseStrategy<GeneralBoard>
    {
        public readonly struct PieceCollapsedEventArgs : ICollapseEventArgs
        {
            public int FromIndex { get; }
            public BoardPiece Piece { get; }
            public CoordsLine Line { get; }

            public PieceCollapsedEventArgs(int fromIndex, BoardPiece piece, CoordsLine line)
            {
                FromIndex = fromIndex;
                Piece = piece;
                Line = line;
            }

            public override string ToString()
            {
                return $"Piece collapsed from {Line.Coords[FromIndex]} to {Piece.Coord} event";
            }
        }

        public readonly struct PieceCreatedEventArgs : IPieceCreatedCollapseEventArgs
        {
            public BoardPiece Piece { get; }
            public int CreateIndex { get; }
            public CoordsLine Line { get; }

            public PieceCreatedEventArgs(BoardPiece piece, int createIndex, CoordsLine line)
            {
                Piece = piece;
                CreateIndex = createIndex;
                Line = line;
            }
            public override string ToString()
            {
                return $"New piece was created at {Piece.Coord} event";
            }
        }

        public override event CollapseEventHandler OnPieceCollapsed;

        private readonly CoordsLine[] lines;
        public IReadOnlyList<CoordsLine> Lines => lines;

        private readonly Dictionary<Vector2Int, Vector2Int> directions;
        public IReadOnlyDictionary<Vector2Int, Vector2Int> Directions => directions;
        
        public LinearGeneralBoardCollapseStrategy(Dictionary<Vector2Int, Vector2Int> directions, CoordsLine[] lines)
        {
            this.directions = new Dictionary<Vector2Int, Vector2Int>(directions);
            this.lines = new CoordsLine[lines.Length];
            System.Array.Copy(lines, this.lines, lines.Length);
        }

        public override bool Collapse(GeneralBoard board, IPieceFactory pieceFactory)
        {
            bool collapsed = false;
            foreach (var line in lines)
            {
                int emptyCellsCount = CollapseTokensInLine(line, board);
                if (emptyCellsCount > 0)
                {
                    collapsed = true;
                    RefillLine(line, emptyCellsCount, board, pieceFactory);
                }
            }

            return collapsed;
        }

        private int CollapseTokensInLine(CoordsLine line, GeneralBoard board)
        {
            int nonExistingPiecesCount = 0;
            for (int index = line.Coords.Count - 1; index >= 0; index--)
            {
                var coord = line.Coords[index];
                var piece = board[coord];
                if (piece == null || piece.IsCleared)
                {
                    nonExistingPiecesCount++;
                }
                else if (nonExistingPiecesCount > 0)
                {
                    var targetCoord = line.Coords[index + nonExistingPiecesCount];
                    board[coord] = null;
                    board[targetCoord] = piece;
                    piece.Coord = targetCoord;
                    OnPieceCollapsed?.Invoke(this, new PieceCollapsedEventArgs(index, piece, line));
                }
            }
            return nonExistingPiecesCount;
        }

        private void RefillLine(CoordsLine line, int count, GeneralBoard board, IPieceFactory pieceFactory)
        {
            int createIndex = 0;
            for (int i = count - 1; i >= 0; i--)
            {
                var coord = line.Coords[i];
                var piece = pieceFactory?.CreatePiece(coord.x, coord.y);
                board[coord] = piece;
                OnPieceCollapsed?.Invoke(this, new PieceCreatedEventArgs(piece, createIndex, line));
                createIndex++;
            }
        }
    }
}
