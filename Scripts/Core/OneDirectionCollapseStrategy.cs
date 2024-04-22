using UnityEngine;

namespace Bipolar.PuzzleBoard
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
            public readonly Vector2Int TargetCoord { get; }

            public PieceCollapsedEventArgs(Piece piece, Vector2Int fromCoord, Vector2Int targetCoord)
            {
                Piece = piece;
                FromCoord = fromCoord;
                TargetCoord = targetCoord;
            }

            public override string ToString()
            {
                return $"Piece collapsed from {FromCoord} to {TargetCoord} event";
            }
        }
    }
}
