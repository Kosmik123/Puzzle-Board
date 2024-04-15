using Bipolar.PuzzleBoard.Rectangular;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [RequireComponent(typeof(RectangularBoardComponent))]
    public class OneDirectionRectangularBoardCollapseController : BoardCollapseController<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    { }
}
