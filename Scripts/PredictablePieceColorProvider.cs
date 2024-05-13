namespace Bipolar.PuzzleBoard
{
    public interface IPredictablePieceColorProvider : IPieceColorProvider
    {
        public int Seed { get; set; }
        public int Time { get; set; }
    }

    public abstract class PredictablePieceColorProvider : PieceColorProvider, IPredictablePieceColorProvider
    {
        public abstract int Time { get; set; }
        public abstract int Seed { get; set; }
    }
}