using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PiecesClearManager : MonoBehaviour
    {
        public event System.Action OnAllPiecesCleared;

        private readonly List<ScenePiece> currentlyClearedPieces = new List<ScenePiece>();

        [SerializeField]
        private SceneBoard board;

        public bool IsClearing => currentlyClearedPieces.Count > 0;

        public void ClearPieces(IReadOnlyList<Piece> pieces)
        {
            foreach (var piece in pieces)
            {
                ClearScenePiece(piece);
            }
        }

        private void ClearScenePiece(Piece piece)
        {
            var scenePiece = board.GetScenePiece(piece);
            currentlyClearedPieces.Add(scenePiece);
            scenePiece.OnCleared += Piece_OnCleared;
            scenePiece.Clear();
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

        private void Piece_OnCleared(ScenePiece piece)
        {
            piece.OnCleared -= Piece_OnCleared;
            currentlyClearedPieces.Remove(piece);
            if (IsClearing == false)
                OnAllPiecesCleared?.Invoke();
        }
    }
}
