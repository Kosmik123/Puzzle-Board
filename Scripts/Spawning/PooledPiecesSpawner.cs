using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Spawning
{
    public class PooledPiecesSpawner : PiecesProvider
    {
        [SerializeField]
        private Piece piecePrototype;
        [SerializeField]
        private Transform piecesContainer;

        private Stack<Piece> piecesPool = new Stack<Piece>();

        protected override Piece Spawn(int x, int y)
        {
            var spawnedPiece = piecesPool.Count > 0 ? piecesPool.Pop() : CreateNewPiece();
            spawnedPiece.IsCleared = false;
            spawnedPiece.gameObject.SetActive(true);
            return spawnedPiece;
        }

        private Piece CreateNewPiece()
        {
            var piece = Instantiate(piecePrototype, piecesContainer);
            piece.OnCleared += Release;
            return piece;
        }

        private void Release(Piece piece)
        {
            piece.gameObject.SetActive(false);
            piecesPool.Push(piece);
        }
    }
}
