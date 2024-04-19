using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class BoardPiece
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

        public BoardPiece (int x, int y)
        {
            coord = new Vector2Int (x, y);
        }

        public void Validate()
        {
            Color = Color;
        }

        public static bool Exists(BoardPiece piece) => piece != null && !piece.IsCleared;
    }

    public abstract class BoardPiece<T> : BoardPiece
        where T : Object, IPieceColor
    {
        [SerializeField]
        private T color;

        protected BoardPiece(int x, int y) : base(x, y)
        { }

        public override IPieceColor Color
        {
            get => color;
            set
            {
                color = (T)value;
                base.Color = color;
            }
        }
    }
}
