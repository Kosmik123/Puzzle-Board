using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Core
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
}
