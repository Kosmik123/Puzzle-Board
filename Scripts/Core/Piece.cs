using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPiece
    {
        event System.Action OnCleared;
        bool IsCleared { get; }
        IPieceColor Color { get; set; }
        void ClearPiece();
    }

    [System.Serializable]
    public abstract class Piece : IPiece
    {
        public event System.Action OnCleared;

        [SerializeField]
        private bool isCleared = false;
        public bool IsCleared => isCleared;

        public virtual IPieceColor Color { get; set; }

        public virtual void ClearPiece()
        {
            isCleared = true;
            OnCleared?.Invoke();
        }

#if UNITY_EDITOR
        internal void Validate()
        {
            Color = Color;
        }
#endif
        public override string ToString()
        {
            return $"Piece ({Color})";
        }
    }

    public class Piece<TColor> : Piece
        where TColor : IPieceColor
    {
        [SerializeField]
        private TColor color;

        public Piece() : base()
        { }

        public override IPieceColor Color
        {
            get => color;
            set
            {
                color = (TColor)value;
                base.Color = color;
            }
        }
    }
}
