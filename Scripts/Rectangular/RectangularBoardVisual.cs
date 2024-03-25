using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    public class RectangularBoardVisual : MonoBehaviour
    {
        [SerializeField]
        private RectangularBoard board;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            if (board)
            {
                RefreshGraphic(board.Dimensions);
                board.OnDimensionsChanged += RefreshGraphic;
            }
        }

        private void RefreshGraphic(Vector2Int dimensions)
        {
            var oneOverParentScale = GetInverseParentScale();
            var rendererScale = Vector3.Scale(board.Grid.cellSize + board.Grid.cellGap, board.transform.lossyScale);
            spriteRenderer.transform.localScale = Vector3.Scale(oneOverParentScale, rendererScale);
            spriteRenderer.size = dimensions;
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

        private void OnDisable()
        {
            if (board)
                board.OnDimensionsChanged -= RefreshGraphic;
        }

        private void OnValidate()
        {
            if (board)
                RefreshGraphic(board.Dimensions);
        }

        private void OnDrawGizmos()
        {
            if (board)
                RefreshGraphic(board.Dimensions);
        }
    }
}
