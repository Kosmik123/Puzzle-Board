using UnityEngine;

namespace Bipolar.PuzzleBoard.Rectangular
{
    public abstract class RectangularBoardVisual : MonoBehaviour
    {
        [SerializeField]
        protected RectangularBoard board;
        [SerializeField]
        protected SpriteRenderer spriteRenderer;
    
        protected abstract void RefreshGraphic(Vector2Int dimensions);
        
        protected virtual void Reset()
        {
            board = FindObjectOfType<RectangularBoard>();
            spriteRenderer = FindObjectOfType<SpriteRenderer>();
        }

        protected virtual void OnEnable()
        {
            RefreshGraphic();
            if (board)
                board.OnDimensionsChanged += RefreshGraphic;
        }

        protected virtual void RefreshGraphic()
        {
            if (board)
                RefreshGraphic(board.Dimensions);
        }

        protected virtual void OnDisable()
        {
            if (board)
                board.OnDimensionsChanged -= RefreshGraphic;
        }

        protected virtual void OnValidate()
        {
            RefreshGraphic();
        }

        protected virtual void OnDrawGizmos()
        {
            RefreshGraphic();
        }
    }
}
