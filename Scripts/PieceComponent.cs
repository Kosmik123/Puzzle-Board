using UnityEngine;

namespace Bipolar.PuzzleBoard
{

    public class PieceComponent : MonoBehaviour
    {
        public event System.Action<IPieceColor> OnColorChanged;
        public event System.Action<PieceComponent> OnCleared;

        [SerializeField]
        private Piece piece;
        public Piece Piece
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
            set => piece.Color = value;
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
                OnColorChanged?.Invoke(piece.Color);
                previousPieceColor = piece.Color;
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

    public abstract class Piece<T> : Piece
        where T : Object, IPieceColor
    {
        [SerializeField]
        private T color;
        public override IPieceColor Color
        {
            get => color;
            set
            {
                color = value as T;
                base.Color = color;
            }
        }
    }
}
