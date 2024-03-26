using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class RandomPieceTypeProvider : PieceTypeProvider
    {
        [SerializeField]
        private Settings settings;

        public override IPieceType GetPieceType(int x, int y)
        {
            return settings.GetPieceType();
        }
    }
}
