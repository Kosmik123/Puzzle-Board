using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class InstantiatingPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private PieceComponent piecePrototype;
        [SerializeField]
        private Transform piecesContainter;

        protected override PieceComponent Spawn(Piece piece)
        {
            var pieceComponent = Instantiate(piecePrototype, piecesContainter);
            pieceComponent.Init(piece);
            pieceComponent.OnCleared += clearedPiece =>
            {
                targetBoard.RemovePieceComponent(clearedPiece);
                Destroy(clearedPiece.gameObject);
            };
            pieceComponent.name = $"{piecePrototype.name} ({piece.Color})";
            return pieceComponent;
        }
    }
}
