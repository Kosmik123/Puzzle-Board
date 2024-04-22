using UnityEngine;

namespace Bipolar.PuzzleBoard.Core
{
    public abstract class OneDirectionCollapseStrategy<TBoard> : BoardCollapseStrategy<TBoard>
        where TBoard : IBoard
    {
        [SerializeField, CollapseDirection]
        private Vector2Int collapseDirection;
        public Vector2Int CollapseDirection => collapseDirection;

        protected int iterationAxis;
        private int collapseAxis;
        public int CollapseAxis => collapseAxis;

        protected void RecalculateAxes()
        {
            collapseAxis = (CollapseDirection.y == 0) ? 0 : 1;
            iterationAxis = 1 - collapseAxis;
        }

        public readonly struct PieceCollapsedEventArgs : ICollapseEventArgs
        {
            public readonly Piece Piece { get; }
            public readonly Vector2Int FromCoord { get; }

            public PieceCollapsedEventArgs(Piece piece, Vector2Int fromCoord)
            {
                Piece = piece;
                FromCoord = fromCoord;
            }

            public override string ToString()
            {
                return $"Piece collapsed from {FromCoord} to {Piece.Coord} event";
            }
        }
    }
}
