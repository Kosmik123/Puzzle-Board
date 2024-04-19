using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceFactory
    {
        public BoardPiece CreatePiece(int x, int y);
    }

    public class PieceFactory : IPieceFactory
    {
        private readonly IPieceColorProvider pieceColorProvider;

        public PieceFactory(IPieceColorProvider pieceColorProvider)
        {
            this.pieceColorProvider = pieceColorProvider;
        }

        public BoardPiece CreatePiece(int x, int y)
        {
            var piece = new BoardPiece(x, y);
            piece.Color = pieceColorProvider.GetPieceColor(x, y);
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
                _pieceFactory ??= new PieceFactory(pieceColorProvider);
                return _pieceFactory;
            }
        }
    }
}
