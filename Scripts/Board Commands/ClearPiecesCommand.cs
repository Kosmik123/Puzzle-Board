using Bipolar.PuzzleBoard.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public struct ClearPiecesCommand : IBoardCommand
    {
        private readonly IReadOnlyList<Piece> piecesToClear;
        private readonly PiecesClearManager piecesClearManager;

        public ClearPiecesCommand(IReadOnlyList<Piece> piecesToClear, PiecesClearManager piecesClearManager)
        {
            this.piecesToClear = piecesToClear;
            this.piecesClearManager = piecesClearManager;
        }

        public IEnumerator Execute()
        {
            piecesClearManager.ClearPieces(piecesToClear);
            yield return new WaitWhile(IsClearing);

            //// tu będzie potrzebny manager clearingu żeby ogarnąć yieldowanie
            //foreach (var piece in piecesToClear)
            //{
            //    var pieceComponent = boardComponent.GetPieceComponent(piece);
            //    pieceComponent.Clear();
            //}

            //yield return new WaitForSeconds(0.3f); // temporary to test
        }

        private bool IsClearing() => piecesClearManager.IsClearing;

        public override string ToString()
        {
            return $"Command to clear {piecesToClear.Count} pieces";
        }
    }
}
