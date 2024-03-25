using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Piece))]
    public abstract class PieceClearingBehavior : MonoBehaviour
    {
        public event System.Action<PieceClearingBehavior> OnClearing;

        private Piece _piece;
        public Piece Piece
        {
            get
            {
                if (_piece == null)
                    _piece = GetComponent<Piece>();
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
}
