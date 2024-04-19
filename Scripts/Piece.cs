using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class Piece
    {
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
        public bool IsCleared
        {
            get => isCleared;
            set
            {
                isCleared = value;
            }
        }

        public virtual IPieceColor Color { get; set; }

        public Piece (int x, int y)
        {
            coord = new Vector2Int (x, y);
        }

        public void Validate()
        {
            Color = Color;
        }

        public static bool Exists(Piece piece) => piece != null && !piece.IsCleared;
    }

    public abstract class Piece<T> : Piece
        where T : Object, IPieceColor
    {
        [SerializeField]
        private T color;

        protected Piece(int x, int y) : base(x, y)
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
