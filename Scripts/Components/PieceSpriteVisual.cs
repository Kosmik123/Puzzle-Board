using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public class PieceSpriteVisual : MonoBehaviour
    {
        [SerializeField]
        private PieceVisualSettings settings;
        [SerializeField] 
        private PieceComponent pieceComponent;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        protected virtual void Reset()
        {
            pieceComponent = GetComponentInParent<PieceComponent>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            pieceComponent.OnColorChanged += RefreshPieceSprite;
            RefreshPieceSprite(pieceComponent.Color);
        }

        private void RefreshPieceSprite(IPieceColor pieceColor)
        {
            if (pieceColor is IVisualPieceColor spritePieceType)
            {
                spriteRenderer.sprite = spritePieceType.Sprite;
                spriteRenderer.color = spritePieceType.Color;
            }
            else if (settings)
            {
                spriteRenderer.color = settings.GetPieceColor(pieceColor);
                var sprite = settings.GetPieceSprite(pieceColor);
                if (sprite)
                    spriteRenderer.sprite = sprite;
            }
        }

        private void OnDisable()
        {
            pieceComponent.OnColorChanged -= RefreshPieceSprite;       
        }
    }
}
