using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Sprite Piece Type")]
    public class SpritePieceType : PieceType, ISpritePieceType
    {
        [SerializeField]
        private Sprite sprite;
        public Sprite Sprite => sprite;

        [SerializeField]
        private Color spriteColor;
        public Color Color => spriteColor;
    }
}

