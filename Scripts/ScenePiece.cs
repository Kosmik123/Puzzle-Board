using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IScenePiece
    {

    }

    [SelectionBase, DisallowMultipleComponent]
    public class ScenePiece : MonoBehaviour, IScenePiece
    {
        public event System.Action<IPieceColor> OnColorChanged;
        public event System.Action<ScenePiece> OnCleared;

        [SerializeReference]
        private IPiece piece;
        public IPiece Piece => piece;

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

        internal void Init(IPiece piece)
        {
            this.piece = piece;
            isCleared = false;
            OnColorChanged?.Invoke(piece.Color);
        }

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (this.piece is Piece piece)
                piece?.Validate();
#endif
        }
    }
}
