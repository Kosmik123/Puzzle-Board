using System.Collections;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PieceComponent))]
    public abstract class PieceClearingBehavior : MonoBehaviour
    {
        public event System.Action<PieceClearingBehavior> OnClearing;

        private PieceComponent _pieceComponent;
        public PieceComponent PieceComponent
        {
            get
            {
                if (_pieceComponent == null)
                    _pieceComponent = GetComponent<PieceComponent>();
                return _pieceComponent;
            }
        }

        protected abstract IEnumerator ClearingProcessCo();

        public void Clear()
        {
            OnClearing?.Invoke(this);
            StartCoroutine(ClearingCo());
        }
        
        private IEnumerator ClearingCo()
        {
            yield return ClearingProcessCo();
            PieceComponent.IsCleared = true;
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
