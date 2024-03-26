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

        private void OnEnable()
        {
            piece.OnTypeChanged += Piece_OnInitialized;       
        }

        private void Piece_OnInitialized(IPieceType type)
        {
            if (type is PieceType pieceType)
            {
                spriteRenderer.color = settings.GetPieceColor(pieceType);
                var sprite = settings.GetPieceSprite(pieceType);
                if (sprite != null)
                    spriteRenderer.sprite = sprite;
            }
        }

        private void OnDisable()
        {
            piece.OnTypeChanged -= Piece_OnInitialized;       
        }
    }
}
