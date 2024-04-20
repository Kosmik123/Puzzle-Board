using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [SelectionBase]
    public class PieceComponent : MonoBehaviour
    {
        public event System.Action<IPieceColor> OnColorChanged;
        public event System.Action<PieceComponent> OnCleared;

        //private IReadOnlyBoard containerBoard;

        [SerializeReference]
        private Piece piece;
        internal Piece Piece => piece;

        [SerializeField]
        private bool isCleared;
        public bool IsCleared
        {
            get => isCleared;
            set
            {
                isCleared = value;
                if (isCleared)
                    OnCleared?.Invoke(this);
            }
        }

        public IPieceColor Color
        {
            get => piece?.Color;
            set
            {
                piece.Color = value;
                previousPieceColor = piece.Color;
                OnColorChanged?.Invoke(piece.Color);
            }
        }

        private IPieceColor previousPieceColor;

        internal void Init(Piece piece)
        {
            this.piece = piece;
            isCleared = false;
        }

        private void Update()
        {
            if (previousPieceColor != piece.Color)
            {
                previousPieceColor = piece.Color;
                OnColorChanged?.Invoke(piece.Color);
            }

            // temp disappearing of Pieces
            if (Piece.IsCleared)
            {
                Destroy(gameObject, 0.3f);
            }
        }

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            piece?.Validate();
#endif
        }
    }
}
