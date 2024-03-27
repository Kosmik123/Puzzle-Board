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
            piece.OnTypeChanged += Piece_OnInitialized;       
        }

        private void Piece_OnInitialized(IPieceType pieceType)
        {
            if (pieceType is ISpritePieceType spritePieceType)
            {
                spriteRenderer.sprite = spritePieceType.Sprite;
                spriteRenderer.color = spritePieceType.Color;
            }
            else if (settings)
            {
                spriteRenderer.color = settings.GetPieceColor(pieceType);
                var sprite = settings.GetPieceSprite(pieceType);
                if (sprite)
                    spriteRenderer.sprite = sprite;
            }
        }

        private void OnDisable()
        {
            piece.OnTypeChanged -= Piece_OnInitialized;       
        }
    }
}
