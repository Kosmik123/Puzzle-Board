using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PiecesSpawner : MonoBehaviour
    {
        public event System.Action<ScenePiece> OnPieceSpawned;

        [SerializeField]
        protected SceneBoard targetBoard;

        public ScenePiece SpawnPiece(Piece piece)
        {
            var pieceComponent = Spawn(piece);
            targetBoard.AddScenePiece(pieceComponent);
            OnPieceSpawned?.Invoke(pieceComponent);
            return pieceComponent;
        }

        protected abstract ScenePiece Spawn(Piece piece);
    }
}
