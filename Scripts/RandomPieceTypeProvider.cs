using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class RandomPieceTypeProvider : PiecesColorProvider
    {
        [SerializeField]
        private PieceColorSettings settings;

        public override IPieceColor GetPieceColor(int x, int y)
        {
            return settings.GetPieceColor();
        }
    }
}
