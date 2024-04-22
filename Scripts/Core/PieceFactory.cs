namespace Bipolar.PuzzleBoard.Core
{
    public interface IPieceFactory
    {
        public Piece CreatePiece(int x, int y);
    }

    public class DefaultPieceFactory : IPieceFactory
    {
        private readonly IPieceColorProvider pieceColorProvider;

        public DefaultPieceFactory(IPieceColorProvider pieceColorProvider)
        {
            this.pieceColorProvider = pieceColorProvider;
        }

        public Piece CreatePiece(int x, int y)
        {
            var pieceColor = pieceColorProvider.GetPieceColor(x, y);
            var piece = new DefaultPiece(x, y, pieceColor);
            return piece;
        }
    }
}