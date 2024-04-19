using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [SelectionBase]
    public class PieceComponent : MonoBehaviour
    {
        public event System.Action<IPieceColor> OnColorChanged;
        public event System.Action<PieceComponent> OnCleared;

        private IReadOnlyBoard containerBoard;

        [SerializeField]
        private Piece piece;
        internal Piece Piece
        {
            get => piece;
            set
            {
                piece = value;
            }
        }

        public bool IsCleared
        {
            get => piece.IsCleared;
            set => piece.IsCleared = value;
        }

        public IPieceColor Color
        {
            get => piece.Color;
            set
            {
                piece.Color = value;
                previousPieceColor = piece.Color;
                OnColorChanged?.Invoke(piece.Color);
            }
        }

        private bool previousCleared;
        private IPieceColor previousPieceColor;

        private void Update()
        {
            if (previousCleared != piece.IsCleared)
            {
                Invoke(nameof(CallClearedEvent), 0);
                previousCleared = piece.IsCleared;
            }

            if (previousPieceColor != piece.Color)
            {
                previousPieceColor = piece.Color;
                OnColorChanged?.Invoke(piece.Color);
            }
        }

        private void CallClearedEvent()
        {
            OnCleared?.Invoke(this);
        }

        protected virtual void OnValidate()
        {
            piece?.Validate();
        }
    }
}
