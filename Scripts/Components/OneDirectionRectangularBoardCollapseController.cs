using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    [RequireComponent(typeof(RectangularBoardComponent))]
    public class OneDirectionRectangularBoardCollapseController : BoardCollapseController<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    { }
}
