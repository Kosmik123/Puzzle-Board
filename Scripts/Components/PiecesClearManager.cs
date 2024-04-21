﻿using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public class PiecesClearManager : MonoBehaviour
    {
        public event System.Action OnAllPiecesCleared;

        [SerializeField]
        private BoardController boardController;

        private readonly List<PieceComponent> currentlyClearedPieces = new List<PieceComponent>();
        public int CurrentlyClearedPiecesCount => currentlyClearedPieces.Count;

        protected virtual void Reset()
        {
            boardController = FindObjectOfType<BoardController>();
        }

        //public void ClearPiecesInChain(PiecesChain chain)
        //{
        //    var clearedPieces = new List<Piece>();
        //    foreach (var coord in chain.PiecesCoords)
        //    {
        //        var piece = boardController.BoardComponent.GetPiece(coord);
        //        piece.Clear();
        //        clearedPieces.Add(piece);
        //    }

        //    var command = new ClearPiecesCommand(clearedPieces, boardController.BoardComponent);
        //    boardController.RequestCommand(command);
        //}

        public void ClearPieces(IReadOnlyList<Piece> pieces)
        {
            foreach (var piece in pieces)
            {
                piece.Clear();
            }

            var command = new ClearPiecesCommand(pieces, boardController.BoardComponent);
            boardController.RequestCommand(command);
        }

        [ContextMenu("Clear queued pieces")]
        private void ClearQueuedPieces()
        {
            foreach (var piece in currentlyClearedPieces)
            {
                if (piece == null) 
                    Debug.LogError("null in chain");

                piece.OnCleared += Piece_OnCleared;
                piece.Clear();
            }
        }

        private void Piece_OnCleared(PieceComponent piece)
        {
            piece.OnCleared -= Piece_OnCleared;
            currentlyClearedPieces.Remove(piece);
            if (currentlyClearedPieces.Count <= 0)
                OnAllPiecesCleared?.Invoke();
        }
    }
}