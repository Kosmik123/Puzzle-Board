using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [SelectionBase, DisallowMultipleComponent]
    public class ScenePiece : MonoBehaviour
    {
        public event System.Action<IPieceColor> OnColorChanged;
        public event System.Action<ScenePiece> OnCleared;

        //private IReadOnlyBoard containerBoard;

        [SerializeReference]
        private Piece piece;
        internal Piece Piece => piece;

        [SerializeField]
        [Tooltip("It's different than Piece.IsCleared")]
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
            CheckColorChange();
        }

        private void Update()
        {
            CheckColorChange();
        }

        private void CheckColorChange()
        {
            if (previousPieceColor != piece.Color)
            {
                previousPieceColor = piece.Color;
                OnColorChanged?.Invoke(piece.Color);
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
