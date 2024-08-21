using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PiecesSpawner : MonoBehaviour
    {
        public event System.Action<ScenePiece> OnPieceSpawned;

        [SerializeField]
        protected SceneBoard targetBoard;

        public ScenePiece SpawnPiece(IPiece piece)
        {
            var pieceComponent = Spawn(piece);
            targetBoard.AddScenePiece(pieceComponent);
            OnPieceSpawned?.Invoke(pieceComponent);
            return pieceComponent;
        }

        protected abstract ScenePiece Spawn(IPiece piece);
    }
}
