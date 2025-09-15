using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [CreateAssetMenu(menuName = Paths.Root + "Sprite Piece Color")]
    public class SpritePieceColor : PieceColor, IVisualPieceColor
    {
        [SerializeField]
        private Sprite sprite;
        public Sprite Sprite => sprite;

        [SerializeField]
        private Color spriteColor;
        public Color Color => spriteColor;
    }
}

