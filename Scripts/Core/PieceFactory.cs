namespace Bipolar.PuzzleBoard
{
    public interface IPieceFactory
    {
        public IPiece CreatePiece(Vector2Int coord);
    }

    public class GenericPieceFactory<TPiece> : IPieceFactory
        where TPiece : IPiece, new()
    {
        private readonly IPieceColorProvider pieceColorProvider;

        public GenericPieceFactory(IPieceColorProvider pieceColorProvider)
        {
            this.pieceColorProvider = pieceColorProvider;
        }

        public virtual IPiece CreatePiece(Vector2Int coord)
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