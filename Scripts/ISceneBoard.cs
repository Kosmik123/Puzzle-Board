using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlySceneBoard
    {
        IReadOnlyBoard Board { get; }
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
    }

    public interface ISceneBoard : IReadOnlySceneBoard
    {
        void AddScenePiece(ScenePiece piece);
        ScenePiece GetScenePiece(Piece piece);
        ScenePiece GetScenePiece(Vector2Int coord);
    }
}
