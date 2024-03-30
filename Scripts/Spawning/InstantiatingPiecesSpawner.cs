using UnityEngine;

namespace Bipolar.PuzzleBoard.Spawning
{
    public class InstantiatingPiecesSpawner : PiecesProvider
    {
        [SerializeField]
        private Piece piecePrototype;
        [SerializeField]
        private Transform piecesContainter;

        [SerializeField]
        private PiecesColorProvider pieceColorProvider;
        public PiecesColorProvider PieceColorProvider
        {
            get => pieceColorProvider;
            set => pieceColorProvider = value;
        }

        protected override Piece Spawn(int x, int y)
        {
            var spawnedPiece = Instantiate(piecePrototype, piecesContainter);
            spawnedPiece.Color = PieceColorProvider.GetPieceColor(x, y);
            spawnedPiece.IsCleared = false;
            spawnedPiece.OnCleared += piece => Destroy(piece.gameObject);
            return spawnedPiece;
        }
    }
}
