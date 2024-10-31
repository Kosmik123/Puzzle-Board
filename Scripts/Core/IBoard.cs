using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlyBoard : IEnumerable<Vector2Int>
    {
        IPiece this[Vector2Int coord] { get; }
        bool ContainsCoord(Vector2Int coord);
        GridLayout.CellLayout Layout { get; }
    }

    public interface IBoard : IReadOnlyBoard
    {
        new IPiece this[Vector2Int coord] { get; set; }
    }

    public static class BoardExtensions
    {
        public static void SwapPieces(this IBoard board, Vector2Int pieceCoord1, Vector2Int pieceCoord2)
        {
            (board[pieceCoord1], board[pieceCoord2]) = (board[pieceCoord2], board[pieceCoord1]);
        }

        public static void SwapPieces(this SceneBoard sceneBoard, Vector2Int pieceCoord1, Vector2Int pieceCoord2)
        {
            var board = sceneBoard.Board;
            (board[pieceCoord1], board[pieceCoord2]) = (board[pieceCoord2], board[pieceCoord1]);
        }
    }
}
