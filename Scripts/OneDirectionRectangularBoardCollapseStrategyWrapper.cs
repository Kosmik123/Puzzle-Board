using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [AddComponentMenu(CreateAssetsPath.Root + nameof(OneDirectionRectangularBoardCollapseStrategy))]
    public class OneDirectionRectangularBoardCollapseStrategyWrapper : BoardCollapseStrategyWrapper<OneDirectionRectangularBoardCollapseStrategy, RectangularBoard>
    { }
}
