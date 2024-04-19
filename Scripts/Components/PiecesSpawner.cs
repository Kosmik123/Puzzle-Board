using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public abstract class PiecesSpawner : MonoBehaviour
    {
        public event System.Action<PieceComponent> OnPieceSpawned;

        [SerializeField]
        protected BoardComponent targetBoard;

        public PieceComponent SpawnPiece(Piece piece)
        {
            var pieceComponent = Spawn(piece);
            targetBoard.AddPieceComponent(pieceComponent);
            OnPieceSpawned?.Invoke(pieceComponent);
            return pieceComponent;
        }

        protected abstract PieceComponent Spawn(Piece piece);
    }
}
