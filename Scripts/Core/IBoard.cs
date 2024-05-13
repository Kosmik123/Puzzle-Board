using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlyBoard : IEnumerable<Vector2Int>
    {
        Piece this[Vector2Int coord] { get; }
        bool ContainsCoord(Vector2Int coord);
        GridLayout.CellLayout Layout { get; }
    }

    public interface IBoard : IReadOnlyBoard
    {
        new Piece this[Vector2Int coord] { get; set; }
    }

    public static class BoardExtensions
    {
        public static void SwapPieces(this IBoard board, Vector2Int pieceCoord1, Vector2Int pieceCoord2)
        {
            (board[pieceCoord1], board[pieceCoord2]) = (board[pieceCoord2], board[pieceCoord1]);
            board[pieceCoord1].Coord = pieceCoord1;
            board[pieceCoord2].Coord = pieceCoord2;
        }
    }
}
