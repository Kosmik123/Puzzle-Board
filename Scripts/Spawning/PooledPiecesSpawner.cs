﻿using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PooledPiecesSpawner : PiecesSpawner
    {
        [SerializeField]
        private PieceComponent piecePrototype;
        [SerializeField]
        private Transform piecesContainer;

        private readonly Stack<PieceComponent> piecesPool = new Stack<PieceComponent>();

        protected override PieceComponent Spawn(Piece piece)
        {
            var spawnedPiece = piecesPool.Count > 0 ? piecesPool.Pop() : CreateNewPiece();
            spawnedPiece.Init(piece);
            spawnedPiece.gameObject.SetActive(true);
            return spawnedPiece;
        }

        private PieceComponent CreateNewPiece()
        {
            var pieceComponent = Instantiate(piecePrototype, piecesContainer);
            pieceComponent.OnCleared += Release;
            return pieceComponent;
        }

        private void Release(PieceComponent piece)
        {
            targetBoard.RemovePieceComponent(piece);
            piece.gameObject.SetActive(false);
            piecesPool.Push(piece);
        }
    }
}