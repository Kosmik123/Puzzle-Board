using PlasticPipe.PlasticProtocol.Messages;
using System;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    public class TiledRectangularBoardVisual : RectangularBoardVisual
    {
        [SerializeField]
        private Vector2 tilesScale = Vector2.one;

        protected override void Reset()
        {
            base.Reset();
            tilesScale = Vector2.one;
        }

        protected override void RefreshGraphic(Vector2Int dimensions)
        {
            var tilesSize = new Vector2(dimensions.x / tilesScale.x, dimensions.y / tilesScale.y);
            var oneOverParentScale = GetInverseParentScale();
            var rendererScale = Vector3.Scale(board.Grid.cellSize + board.Grid.cellGap, board.transform.lossyScale);
            rendererScale.Scale(tilesScale);

            spriteRenderer.transform.localScale = Vector3.Scale(oneOverParentScale, rendererScale);
            spriteRenderer.size = tilesSize;
        }

        private Vector3 GetInverseParentScale()
        {
            if (spriteRenderer.transform.parent == null)
                return Vector3.one;

            var parentScale = spriteRenderer.transform.parent.lossyScale;
            var oneOverParentScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z);
            return oneOverParentScale;
        }
    }
}
