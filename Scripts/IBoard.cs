using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IBoardState : IEnumerable<Vector2Int>
    {
        Piece this[Vector2Int coord] { get; }
        GridLayout.CellLayout Layout { get; }
        bool ContainsCoord(Vector2Int coord);
    }

    public interface IBoard : IBoardState
    {
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
    }

    public interface IModifiableBoard : IBoard
    {
        new Piece this[Vector2Int coord] { get; set; }
    }
}
