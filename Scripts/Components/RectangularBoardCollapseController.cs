using Bipolar.PuzzleBoard.Rectangular;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [RequireComponent(typeof(RectangularBoardComponent))]
    public class RectangularBoardCollapseController : BoardCollapseController<RectangularBoardComponent, RectangularBoard>
    {
        public override event System.Action OnPiecesColapsed;

        public override bool IsCollapsing => false;
    }
}
