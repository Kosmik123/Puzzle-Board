using UnityEngine;
using UnityEngine.UI;

namespace Bipolar.PuzzleBoard.Rectangular
{
    public class TiledImageRectangularBoardVisual : RectangularBoardVisual
    {
        [SerializeReference]
        private Image image;

        protected override void Reset()
        {
            base.Reset();
            image = GetComponentInChildren<Image>();
        }

        [ContextMenu("Refresh")]
        private void Refresh() => RefreshGraphic(board.Dimensions);
        
        protected override void RefreshGraphic(Vector2Int dimensions)
        {
            var tilesSize = board.Grid.cellSize + board.Grid.cellGap;
            var size = Vector2.Scale(dimensions, tilesSize);
            var pixelsPerUnit = image.canvas.referencePixelsPerUnit;
            var imageScale = tilesSize / pixelsPerUnit;

            var rectTransform = image.rectTransform;
            rectTransform.localScale = imageScale;
            rectTransform.sizeDelta = size / imageScale;
        }
    }
}
