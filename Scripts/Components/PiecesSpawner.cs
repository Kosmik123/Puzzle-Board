using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public abstract class PiecesSpawner : MonoBehaviour
    {
        public event System.Action<Piece> OnPieceSpawned;

        [SerializeField]
        protected BoardComponent targetBoard;

        public Piece SpawnPiece(BoardPiece piece)
        {
            var pieceComponent = Spawn(piece);
            targetBoard.AddPiece(pieceComponent);
            OnPieceSpawned?.Invoke(pieceComponent);
            return pieceComponent;
        }

        protected abstract Piece Spawn(BoardPiece piece);
    }
}
