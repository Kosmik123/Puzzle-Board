using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public abstract class RectangularBoardVisual : MonoBehaviour
    {
        [SerializeField]
        protected RectangularBoardComponent boardComponent;
        [SerializeField]
        protected SpriteRenderer spriteRenderer;
    
        protected abstract void RefreshGraphic(Vector2Int dimensions);
        
        protected virtual void Reset()
        {
            boardComponent = FindObjectOfType<RectangularBoardComponent>();
            spriteRenderer = FindObjectOfType<SpriteRenderer>();
        }

        protected virtual void OnEnable()
        {
            RefreshGraphic();
            if (boardComponent)
                boardComponent.OnDimensionsChanged += RefreshGraphic;
        }

        protected virtual void RefreshGraphic()
        {
            if (boardComponent)
                RefreshGraphic(boardComponent.Dimensions);
        }

        protected virtual void OnDisable()
        {
            if (boardComponent)
                boardComponent.OnDimensionsChanged -= RefreshGraphic;
        }

        protected virtual void OnDrawGizmos()
        {
            RefreshGraphic();
        }
    }
}
