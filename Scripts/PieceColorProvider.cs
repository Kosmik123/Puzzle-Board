using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PieceColorProvider : MonoBehaviour, IPieceColorProvider
    {
        public abstract IPieceColor GetPieceColor(int x, int y);
    }

    public abstract class PredictablePieceColorProvider : PieceColorProvider, IPredictablePieceColorProvider
    {
        public abstract int Time { get; set; }
        public abstract int Seed { get; set; }
    }
}
