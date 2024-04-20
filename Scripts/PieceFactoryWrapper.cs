using UnityEngine;

namespace Bipolar.PuzzleBoard
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

    public class PieceFactoryWrapper : MonoBehaviour
    {
        [SerializeField]
        private PieceColorProvider pieceColorProvider;

        private IPieceFactory _pieceFactory;
        public IPieceFactory PieceFactory
        {
            get
            {
                _pieceFactory ??= new DefaultPieceFactory(pieceColorProvider);
                return _pieceFactory;
            }
        }
    }
}
