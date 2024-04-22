using UnityEngine;

namespace Bipolar.PuzzleBoard.Core
{
    [System.Serializable]
    public class Piece
    {
        public event System.Action OnCleared;

        [SerializeField]
        private Vector2Int coord;
        public Vector2Int Coord
        {
            get => coord;
            internal set
            {
                coord = value;
            }
        }

        [SerializeField]
        private bool isCleared = false;
        public bool IsCleared => isCleared;

       // private IBoard containingBoard;

        public static bool Exists(Piece piece) => piece != null && !piece.IsCleared;
        public virtual IPieceColor Color { get; set; }

        public Piece (int x, int y/*, IBoard board*/, IPieceColor color)
        {
            //containingBoard = board;
            //board[coord] = this;
            coord = new Vector2Int (x, y);
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

    public abstract class Piece<T> : Piece
        where T : Object, IPieceColor
    {
        [SerializeField]
        private T color;

        protected Piece(int x, int y, IPieceColor color) : base(x, y, color)
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
