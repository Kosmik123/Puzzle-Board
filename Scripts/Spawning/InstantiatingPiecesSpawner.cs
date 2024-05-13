using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class InstantiatingPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private ScenePiece piecePrototype;

        protected override ScenePiece Spawn(Piece piece)
        {
            var pieceComponent = Instantiate(piecePrototype, targetBoard.transform);
            pieceComponent.Init(piece);
            pieceComponent.OnCleared += clearedPiece =>
            {
                targetBoard.RemoveScenePiece(clearedPiece);
                Destroy(clearedPiece.gameObject);
            };
            pieceComponent.name = $"{piecePrototype.name} ({piece.Color})";
            return pieceComponent;
        }
    }
}
