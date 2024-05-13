using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    [RequireComponent(typeof(RectangularSceneBoard))]
    public class OneDirectionRectangularBoardCollapseController : BoardCollapseController<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    { }
}
