using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class RectangularBoardData : BoardData
    {
        private Piece[,] piecesArray;
     
        public RectangularBoardData (int width, int height)
        {
            piecesArray = new Piece[width, height];
        }
        
        public override Piece this[Vector2Int coord]
        {
            get => piecesArray[coord.x, coord.y];
            set => piecesArray[coord.x, coord.y] = value;
        }
    }
}
