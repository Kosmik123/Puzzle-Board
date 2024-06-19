namespace Bipolar.PuzzleBoard
{
    public interface IPieceFactory
    {
        public Piece CreatePiece(int x, int y);
    }

    public class GenericPieceFactory<TPiece> : IPieceFactory
        where TPiece : Piece
    {
        private readonly IPieceColorProvider pieceColorProvider;

        public GenericPieceFactory(IPieceColorProvider pieceColorProvider)
        {
            this.pieceColorProvider = pieceColorProvider;
        }

        public Piece CreatePiece(int x, int y)
        {
            var pieceColor = pieceColorProvider.GetPieceColor(x, y);
            var piece = (TPiece)System.Activator.CreateInstance(typeof(TPiece), pieceColor);
            return piece;
        }
    }

    public class DefaultPieceFactory : GenericPieceFactory<DefaultPiece>
    {
        public DefaultPieceFactory(IPieceColorProvider pieceColorProvider) : base(pieceColorProvider)
        { }
    }
}