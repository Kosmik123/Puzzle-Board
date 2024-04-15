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

        private IPieceColor pieceColor;
        public virtual IPieceColor Color
        {
            get => pieceColor;
            set
            {
                pieceColor = value;
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

        public Piece (int x, int y)
        {
            coord = new Vector2Int (x, y);
        }

        public void Validate()
        {
            Color = Color;
        }
    }
}
