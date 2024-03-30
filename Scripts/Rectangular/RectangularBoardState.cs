using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class RectangularBoardState : BoardState
    {
        private readonly int width;
        private readonly int height;
        private readonly Piece[,] piecesArray;

        public override Piece this[Vector2Int coord]
        {
            get => piecesArray[coord.x, coord.y];
            set => piecesArray[coord.x, coord.y] = value;
        }

        public RectangularBoardState (int width, int height, GridLayout.CellLayout layout) : base(layout)
        {
            this.width = width;
            this.height = height;
            piecesArray = new Piece[width, height];
        }

        public RectangularBoardState (RectangularBoardState source) : base(source.Layout) 
        {
            width = source.width;
            height = source.height;
            piecesArray = (Piece[,])source.piecesArray.Clone();
        }

        public override bool ContainsCoord(int xCoord, int yCoord)
        {
            if (xCoord < 0 || yCoord < 0)
                return false;

            if (xCoord >= piecesArray.GetLength(0) || yCoord >= piecesArray.GetLength(1))
                return false;

            return true;
        }
    }
}
