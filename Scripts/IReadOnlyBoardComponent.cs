using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlyBoard : IEnumerable<Vector2Int>
    {
        Piece this[Vector2Int coord] { get; }
    }

    public interface IBoard : IReadOnlyBoard
    {
        new Piece this[Vector2Int coord] { get; set; }
        GridLayout.CellLayout Layout { get; }
        bool ContainsCoord(Vector2Int coord);
    }

    public interface IReadOnlyBoardComponent
    {
        IBoard Board { get; }
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
    }

    public interface IBoardComponent : IReadOnlyBoardComponent
    {
        void AddPiece(PieceComponent piece);
    }
}
