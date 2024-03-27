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
            public PieceType type;
            public Color color = Color.white;
            public Sprite sprite;
        }

        [SerializeField]
        private PieceVisualMapping[] pieceVisualMappings;

        [SerializeField]
        private Sprite defaultSprite;

        private Dictionary<IPieceType, Color> pieceVisualColors;
        private Dictionary<IPieceType, Sprite> pieceVisualSprites;

        public Color GetPieceColor(IPieceType type)
        {
            if (pieceVisualColors == null)
            {
                pieceVisualColors = new Dictionary<IPieceType, Color>();
                foreach (var mapping in pieceVisualMappings)
                    pieceVisualColors[mapping.type] = mapping.color;
            }

            return pieceVisualColors.TryGetValue(type, out var color) ? color : Color.white;
        }

        public Sprite GetPieceSprite(IPieceType type)
        {
            if (pieceVisualSprites == null)
            {
                pieceVisualSprites = new Dictionary<IPieceType, Sprite>();
                foreach (var mapping in pieceVisualMappings)
                    pieceVisualSprites[mapping.type] = mapping.sprite;
            }

            return pieceVisualSprites.TryGetValue(type, out var sprite) ? sprite : defaultSprite;
        }
    }
}
