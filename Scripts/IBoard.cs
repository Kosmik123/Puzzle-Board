using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlyBoard : IEnumerable<Vector2Int>
    {
        BoardPiece this[Vector2Int coord] { get; }
    }

    public interface IBoard : IReadOnlyBoard
    {
        new BoardPiece this[Vector2Int coord] { get; set; }
        GridLayout.CellLayout Layout { get; }
        bool ContainsCoord(Vector2Int coord);
    }
}
