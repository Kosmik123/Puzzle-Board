using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IBoard : IEnumerable<Vector2Int>
    {
        PieceComponent this[Vector2Int coord] { get; }
        GridLayout.CellLayout Layout { get; }
        bool ContainsCoord(Vector2Int coord);
    }

    public interface IReadOnlyBoardComponent : IBoard
    {
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
    }

    public interface IBoardComponent : IReadOnlyBoardComponent
    {
        new PieceComponent this[Vector2Int coord] { get; set; }
    }
}
