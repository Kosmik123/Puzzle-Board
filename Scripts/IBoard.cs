using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IBoard
    {
        BoardData Data { get; }
        Piece this[Vector2Int coord] { get; }
        bool ContainsCoord(Vector2Int coord);
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
        Piece GetPiece(Vector2Int coord);
        Piece GetPiece(int x, int y);
    }

    public interface IModifiableBoard : IBoard
    {
        new Piece this[Vector2Int coord] { get; set; }
    }
}
