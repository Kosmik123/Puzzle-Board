namespace Bipolar.PuzzleBoard
{
    public interface IPieceColorProvider
    {
        IPieceColor GetPieceColor(int x, int y);
    }

    public interface IPredictablePieceColorProvider : IPieceColorProvider
    {
        public int Seed { get; set; }
        public int Time { get; set; }
    }
}