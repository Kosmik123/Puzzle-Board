using UnityEngine;

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
            var pieceColor = pieceColorProvider.GetPieceColor(coord.x, coord.y);
            var piece = new TPiece();
            piece.Color = pieceColor;
            return piece;
        }
    }

    public class DefaultPieceFactory : GenericPieceFactory<DefaultPiece>
    {
        public DefaultPieceFactory(IPieceColorProvider pieceColorProvider) 
            : base(pieceColorProvider)
        { }
    }
}