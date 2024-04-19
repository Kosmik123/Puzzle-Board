using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
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
