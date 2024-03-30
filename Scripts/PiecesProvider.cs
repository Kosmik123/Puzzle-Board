using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PiecesProvider : MonoBehaviour
    {
        public event System.Action<Piece> OnPieceSpawned;

        public Piece SpawnPiece(int xCoord, int yCoord)
        {
            var piece = Spawn(xCoord, yCoord);
            OnPieceSpawned?.Invoke(piece);
            return piece;
        }

        protected abstract Piece Spawn(int xCoord, int yCoord);
    }
}
