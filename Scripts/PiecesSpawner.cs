using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PiecesSpawner : MonoBehaviour
    {
        public event System.Action<PieceComponent> OnPieceSpawned;

        public PieceComponent SpawnPiece(int xCoord, int yCoord)
        {
            var piece = Spawn(xCoord, yCoord);
            OnPieceSpawned?.Invoke(piece);
            return piece;
        }

        protected abstract PieceComponent Spawn(int xCoord, int yCoord);
    }
}
