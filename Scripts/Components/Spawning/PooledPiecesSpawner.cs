using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public class PooledPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private Piece piecePrototype;
        [SerializeField]
        private Transform piecesContainer;

        private Stack<Piece> piecesPool = new Stack<Piece>();

        protected override Piece Spawn(BoardPiece piece)
        {
            var spawnedPiece = piecesPool.Count > 0 ? piecesPool.Pop() : CreateNewPiece();
            spawnedPiece.BoardPiece = piece;
            spawnedPiece.IsCleared = false;
            spawnedPiece.gameObject.SetActive(true);
            return spawnedPiece;
        }

        private Piece CreateNewPiece()
        {
            var pieceComponent = Instantiate(piecePrototype, piecesContainer);
            pieceComponent.OnCleared += Release;
            return pieceComponent;
        }

        private void Release(Piece piece)
        {
            targetBoard.RemovePiece(piece);
            piece.gameObject.SetActive(false);
            piecesPool.Push(piece);
        }
    }
}
