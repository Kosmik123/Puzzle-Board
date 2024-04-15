using UnityEngine;

namespace Bipolar.PuzzleBoard.Spawning
{
    public class InstantiatingPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private PieceComponent piecePrototype;
        [SerializeField]
        private Transform piecesContainter;

        [SerializeField]
        private PieceColorProvider pieceColorProvider;
        public PieceColorProvider PieceColorProvider
        {
            get => pieceColorProvider;
            set => pieceColorProvider = value;
        }

        protected override PieceComponent Spawn(Piece piece)
        {
            var pieceComponent = Instantiate(piecePrototype, piecesContainter);
            pieceComponent.Piece = piece;
            pieceComponent.Color = PieceColorProvider.GetPieceColor(piece.Coord.x, piece.Coord.y);
            pieceComponent.IsCleared = false;
            pieceComponent.OnCleared += clearedPiece => Destroy(clearedPiece.gameObject);
            pieceComponent.name = $"{piecePrototype.name} ({piece.Color})";
            return pieceComponent;
        }
    }
}
