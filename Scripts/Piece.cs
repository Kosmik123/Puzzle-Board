using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class Piece : MonoBehaviour
    {
        public event System.Action<IPieceColor> OnColorChanged;
        public event System.Action<Piece> OnCleared;

        private IPieceColor pieceColor;
        public virtual IPieceColor Color 
        {
            get => pieceColor;
            set 
            {
                pieceColor = value;
                OnColorChanged?.Invoke(value);
            } 
        }

        private bool isCleared = false;
        public bool IsCleared
        {
            get => isCleared;
            set
            {
                isCleared = value;
                if (isCleared)
                    Invoke(nameof(CallClearedEvent), 0);
            }
        }

        private void CallClearedEvent()
        {
            OnCleared?.Invoke(this);
        }

        protected virtual void OnValidate()
        {
            Color = Color;
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
                if (color)
                    gameObject.name = $"Piece ({color.name})";
                base.Color = color;
            }
        }
    }
}
