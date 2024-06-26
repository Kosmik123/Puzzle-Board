﻿using System.Collections.Generic;
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
        private readonly Piece[,] piecesArray;

        public Vector2Int Dimensions => new Vector2Int(width, height);

        public override Piece this[Vector2Int coord]
        {
            get => piecesArray[coord.x, coord.y];
            set => piecesArray[coord.x, coord.y] = value;
        }

        public RectangularBoard (int width, int height, GridLayout.CellLayout layout) : base(layout)
        {
            this.width = width;
            this.height = height;
            piecesArray = new Piece[width, height];
        }

        private RectangularBoard (RectangularBoard source) : base(source.Layout) 
        {
            width = source.width;
            height = source.height;
            piecesArray = (Piece[,])source.piecesArray.Clone();
        }

        public override bool ContainsCoord(int xCoord, int yCoord)
        {
            if (xCoord < 0 || yCoord < 0)
                return false;

            if (xCoord >= width || yCoord >= height)
                return false;

            return true;
        }

        public override Board Clone() => new RectangularBoard(this);

        public override IEnumerator<Vector2Int> GetEnumerator()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    yield return new Vector2Int(x, y);
        }

        protected override void CopyState(IBoard target)
        {
            if (!(target is RectangularBoard rectangularTarget))
                throw new System.InvalidCastException();

            int width = Mathf.Min(this.width, rectangularTarget.width);
            int height = Mathf.Min(this.height, rectangularTarget.height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    rectangularTarget.piecesArray[x, y] = piecesArray[x, y];
        }
    }
}
