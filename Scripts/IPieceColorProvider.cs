namespace Bipolar.PuzzleBoard
{
    public interface IPieceColorProvider
    {
        IPieceColor GetPieceColor(int x, int y);
    }
}