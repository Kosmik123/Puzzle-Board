using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Components
{
    public class PiecesClearManager : MonoBehaviour
    {
        public event System.Action OnAllPiecesCleared;

        private readonly List<PieceComponent> currentlyClearedPieces = new List<PieceComponent>();

        [SerializeField]
        private BoardComponent boardComponent;

        public bool IsClearing => currentlyClearedPieces.Count > 0;

        public void ClearPieces(IReadOnlyList<Piece> pieces)
        {
            foreach (var piece in pieces)
            {
                ClearPieceComponent(piece);
            }
        }

        private void ClearPieceComponent(Piece piece)
        {
            var pieceComponent = boardComponent.GetPieceComponent(piece);
            currentlyClearedPieces.Add(pieceComponent);
            pieceComponent.OnCleared += Piece_OnCleared;
            pieceComponent.Clear();
        }

        //[ContextMenu("Clear queued pieces")]
        //private void ClearQueuedPieces()
        //{
        //    foreach (var piece in currentlyClearedPieces)
        //    {
        //        if (piece == null) 
        //            Debug.LogError("null in chain");

        //        piece.OnCleared += Piece_OnCleared;
        //        piece.Clear();
        //    }
        //}

        private void Piece_OnCleared(PieceComponent piece)
        {
            piece.OnCleared -= Piece_OnCleared;
            currentlyClearedPieces.Remove(piece);
            if (IsClearing == false)
                OnAllPiecesCleared?.Invoke();
        }
    }
}
