using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Piece Visual Settings")]
    public class PieceVisualSettings : ScriptableObject
    {
        [System.Serializable]
        public class PieceVisualMapping
        {
            public PieceColor type;
            public Color color = Color.white;
            public Sprite sprite;
        }

        [SerializeField]
        private PieceVisualMapping[] pieceVisualMappings;

        [SerializeField]
        private Sprite defaultSprite;

        private Dictionary<IPieceColor, Color> pieceVisualColors;
        private Dictionary<IPieceColor, Sprite> pieceVisualSprites;

        public Color GetPieceColor(IPieceColor pieceColor)
        {
            if (pieceVisualColors == null)
            {
                pieceVisualColors = new Dictionary<IPieceColor, Color>();
                foreach (var mapping in pieceVisualMappings)
                    pieceVisualColors[mapping.type] = mapping.color;
            }

            return pieceColor != null && pieceVisualColors.TryGetValue(pieceColor, out var color) ? color : Color.white;
        }

        public Sprite GetPieceSprite(IPieceColor pieceColor)
        {
            if (pieceVisualSprites == null)
            {
                pieceVisualSprites = new Dictionary<IPieceColor, Sprite>();
                foreach (var mapping in pieceVisualMappings)
                    pieceVisualSprites[mapping.type] = mapping.sprite;
            }

            return pieceColor != null && pieceVisualSprites.TryGetValue(pieceColor, out var sprite) ? sprite : defaultSprite;
        }
    }
}
