using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    public interface IRectangularBoard : IBoard
    {
        Vector2Int Dimensions { get; }
    }

    [System.Serializable]
    public class RectangularBoard : Board, IRectangularBoard
    {
        private readonly int width;
        private readonly int height;
        private readonly IPiece[,] piecesArray;

        public Vector2Int Dimensions => new Vector2Int(width, height);

        public override IPiece this[Vector2Int coord]
        {
            get => piecesArray[coord.x, coord.y];
            set => piecesArray[coord.x, coord.y] = value;
        }

        public RectangularBoard (int width, int height, GridLayout.CellLayout layout) : base(layout)
        {
            this.width = width;
            this.height = height;
            piecesArray = new IPiece[width, height];
        }

		private RectangularBoard(RectangularBoard source) 
            : this(source.width, source.height, source.Layout)
            => System.Array.Copy(source.piecesArray, piecesArray, source.width * source.height);  

		public override bool ContainsCoord(int xCoord, int yCoord) =>
            xCoord >= 0 && yCoord >= 0 && xCoord < width && yCoord < height; 

        public override Board Clone() => new RectangularBoard(this);

        public override IEnumerator<Vector2Int> GetEnumerator()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    yield return new Vector2Int(x, y);
        }

        protected override void CopyState(IBoard target)
		{
            if (target is RectangularBoard rectangularTarget)
            {
                System.Array.Copy(piecesArray, rectangularTarget.piecesArray, width * height);  
			}
            else
            {
                throw new System.ArgumentException();
            }
		}
	}
}
