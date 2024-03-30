using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PieceSpriteVisual : MonoBehaviour
    {
        [SerializeField]
        private PieceVisualSettings settings;
        [SerializeField] 
        private Piece piece;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        protected virtual void Reset()
        {
            piece = GetComponentInParent<Piece>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            piece.OnColorChanged += Piece_OnInitialized;       
        }

        private void Piece_OnInitialized(IPieceColor pieceColor)
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
            piece.OnColorChanged -= Piece_OnInitialized;       
        }
    }
}
