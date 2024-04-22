using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    [RequireComponent(typeof(RectangularBoardComponent))]
    public class OneDirectionRectangularBoardCollapseController : BoardCollapseController<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    { }
}
