using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PooledPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private ScenePiece piecePrototype;

        private readonly Stack<ScenePiece> piecesPool = new Stack<ScenePiece>();

        [SerializeField]
        private int pooledCount;

        protected override ScenePiece Spawn(IPiece piece)
        {
            var spawnedPiece = piecesPool.Count > 0 ? piecesPool.Pop() : CreateNewPiece();
            spawnedPiece.Init(piece);
            spawnedPiece.gameObject.SetActive(true);
            UpdatePooledCount();
            return spawnedPiece;
        }

        private ScenePiece CreateNewPiece()
        {
            var pieceComponent = Instantiate(piecePrototype, targetBoard.transform);
            pieceComponent.OnCleared += Release;
            return pieceComponent;
        }

        private void Release(ScenePiece piece)
        {
            targetBoard.RemoveScenePiece(piece);
            piece.gameObject.SetActive(false);
            piecesPool.Push(piece);
            UpdatePooledCount();
        }

        private void UpdatePooledCount()
        {
            pooledCount = piecesPool.Count;
        }
    }
}
