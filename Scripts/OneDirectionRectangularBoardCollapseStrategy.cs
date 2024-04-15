using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class OneDirectionRectangularBoardCollapseStrategy : BoardCollapseStrategy<RectangularBoard>
    {
        [SerializeField, CollapseDirection]
        private Vector2Int collapseDirection;
        public Vector2Int CollapseDirection => collapseDirection;

        private int iterationAxis;
        private int collapseAxis;

        protected override void Collapse(RectangularBoard board)
        {

        }
    }
}
