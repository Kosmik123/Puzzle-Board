using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class RandomPieceTypeProvider : PiecesColorProvider
    {
        [SerializeField]
        private Settings settings;

        public override IPieceColor GetPieceColor(int x, int y)
        {
            return settings.GetPieceType();
        }
    }
}
