using Bipolar.PuzzleBoard.General;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Core
{
    public class LinearGeneralBoardCollapseStrategy : BoardCollapseStrategy<GeneralBoard>
    {
        public readonly struct PieceCollapsedEventArgs : ICollapseEventArgs
        {
            public int FromIndex { get; }
            public Piece Piece { get; }
            public CoordsLine Line { get; }
            public Vector2Int TargetCoord { get; }

            public PieceCollapsedEventArgs(int fromIndex, Piece piece, CoordsLine line, Vector2Int targetCoord)
            {
                FromIndex = fromIndex;
                Piece = piece;
                Line = line;
                TargetCoord = targetCoord;
            }

            public override string ToString()
            {
                return $"Piece collapsed from {Line.Coords[FromIndex]} to {TargetCoord} event";
            }
        }

        public readonly struct PieceCreatedEventArgs : IPieceCreatedCollapseEventArgs
        {
            public Piece Piece { get; }
            public int CreateIndex { get; }
            public CoordsLine Line { get; }
            public Vector2Int CreationCoord { get; }

            public PieceCreatedEventArgs(Piece piece, int createIndex, CoordsLine line, Vector2Int creationCoord)
            {
                Piece = piece;
                CreateIndex = createIndex;
                Line = line;
                CreationCoord = creationCoord;
            }

            public override string ToString()
            {
                return $"New piece was created at {CreationCoord} event";
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
                    int targetIndex = index + nonExistingPiecesCount;
                    var targetCoord = line.Coords[targetIndex];
                    board[coord] = null;
                    board[targetCoord] = piece;
                    piece.Coord = targetCoord;
                    OnPieceCollapsed?.Invoke(this, new PieceCollapsedEventArgs(index, piece, line, targetCoord));
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
                OnPieceCollapsed?.Invoke(this, new PieceCreatedEventArgs(piece, createIndex, line, coord));
                createIndex++;
            }
        }
    }
}
