using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PieceSpriteVisual : MonoBehaviour
    {
        [SerializeField]
        private PieceVisualSettings settings;
        [SerializeField] 
        private ScenePiece scenePiece;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        protected virtual void Reset()
        {
            scenePiece = GetComponentInParent<ScenePiece>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            scenePiece.OnColorChanged += RefreshPieceSprite;
            RefreshPieceSprite(scenePiece.Color);
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
            scenePiece.OnColorChanged -= RefreshPieceSprite;       
        }
    }
}
