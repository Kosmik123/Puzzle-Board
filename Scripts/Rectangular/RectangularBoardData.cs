using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class RectangularBoardData : BoardData
    {
        private readonly int width;
        private readonly int height;
        private readonly Piece[,] piecesArray;

        public override Piece this[Vector2Int coord]
        {
            get => piecesArray[coord.x, coord.y];
            set => piecesArray[coord.x, coord.y] = value;
        }

        public RectangularBoardData (int width, int height, GridLayout.CellLayout layout) : base(layout)
        {
            this.width = width;
            this.height = height;
            piecesArray = new Piece[width, height];
        }

        public override bool ContainsCoord(int xCoord, int yCoord)
        {
            if (xCoord < 0 || yCoord < 0)
                return false;

            if (xCoord >= width || yCoord >= height)
                return false;

            return true;
        }
    }
}
