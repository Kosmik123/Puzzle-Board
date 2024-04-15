using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PiecesSpawner : MonoBehaviour
    {
        public event System.Action<PieceComponent> OnPieceSpawned;

        public PieceComponent SpawnPiece(Piece piece)
        {
            var pieceComponent = Spawn(piece);
            OnPieceSpawned?.Invoke(pieceComponent);
            return pieceComponent;
        }

        protected abstract PieceComponent Spawn(Piece piece);
    }
}
