using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class Piece
    {
        public event System.Action OnCleared;

        [SerializeField]
        private bool isCleared = false;
        public bool IsCleared => isCleared;

        public static bool Exists(Piece piece) => piece != null && !piece.IsCleared;
        public virtual IPieceColor Color { get; set; }

        public Piece (IPieceColor color)
        {
            Color = color;
        }

        public void Clear()
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

    public class Piece<T> : Piece
        where T : Object, IPieceColor
    {
        [SerializeField]
        private T color;

        public Piece(IPieceColor color) : base(color)
        { }

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
