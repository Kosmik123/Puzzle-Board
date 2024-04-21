using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public interface IReadOnlyBoardComponent
    {
        IReadOnlyBoard Board { get; }
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
    }

    public interface IBoardComponent : IReadOnlyBoardComponent
    {
        void AddPieceComponent(PieceComponent piece);
        PieceComponent GetPieceComponent(Piece piece);
        PieceComponent GetPieceComponent(Vector2Int coord);
    }
}
