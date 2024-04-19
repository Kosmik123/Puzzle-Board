using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PieceComponent))]
    public abstract class PieceClearingBehavior : MonoBehaviour
    {
        public event System.Action<PieceClearingBehavior> OnClearing;

        private PieceComponent _piece;
        public PieceComponent Piece
        {
            get
            {
                if (_piece == null)
                    _piece = GetComponent<PieceComponent>();
                return _piece;
            }
        }

        protected abstract void ClearPiece();

        public void Clear()
        {
            OnClearing?.Invoke(this);
            ClearPiece();
        }

        protected void FinishClearing()
        {
            Piece.IsCleared = true;
        }
    }

    public static class PieceClearingExtension
    {
        public static void Clear(this PieceComponent piece)
        {
            if (piece.TryGetComponent<PieceClearingBehavior>(out var pieceClearing))
            {
                pieceClearing.Clear();
            }
            else
            {
                piece.IsCleared = true;
            }
        }
    }
}
