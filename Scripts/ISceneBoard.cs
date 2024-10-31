using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlySceneBoard
    {
        IBoard Board { get; }
        Vector3 CoordToWorld(Vector2 coord);
        Vector3 CoordToWorld(float x, float y);
    }

    public interface ISceneBoard : IReadOnlySceneBoard
    {
        void AddScenePiece(ScenePiece piece);
        ScenePiece GetScenePiece(IPiece piece);
        ScenePiece GetScenePiece(Vector2Int coord);
    }
}
