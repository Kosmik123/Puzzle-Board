using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public class InstantiatingPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private Piece piecePrototype;
        [SerializeField]
        private Transform piecesContainter;

        [SerializeField]
        private PieceColorProvider pieceColorProvider;
        public PieceColorProvider PieceColorProvider
        {
            get => pieceColorProvider;
            set => pieceColorProvider = value;
        }

        protected override Piece Spawn(BoardPiece piece)
        {
            var pieceComponent = Instantiate(piecePrototype, piecesContainter);
            pieceComponent.BoardPiece = piece;
            pieceComponent.Color = PieceColorProvider.GetPieceColor(piece.Coord.x, piece.Coord.y);
            pieceComponent.IsCleared = false;
            pieceComponent.OnCleared += clearedPiece =>
            {
                targetBoard.RemovePiece(clearedPiece);
                Destroy(clearedPiece.gameObject);
            };
            pieceComponent.name = $"{piecePrototype.name} ({piece.Color})";
            return pieceComponent;
        }
    }
}
