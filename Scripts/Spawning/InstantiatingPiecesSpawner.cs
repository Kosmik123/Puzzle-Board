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
        private PiecesColorProvider pieceColorProvider;
        public PiecesColorProvider PieceColorProvider
        {
            get => pieceColorProvider;
            set => pieceColorProvider = value;
        }

        protected override PieceComponent Spawn(int x, int y)
        {
            var pieceComponent = Instantiate(piecePrototype, piecesContainter);
            pieceComponent.Piece = new Piece(x, y);
            pieceComponent.Color = PieceColorProvider.GetPieceColor(x, y);
            pieceComponent.IsCleared = false;
            pieceComponent.OnCleared += piece => Destroy(piece.gameObject);
            return pieceComponent;
        }
    }
}
