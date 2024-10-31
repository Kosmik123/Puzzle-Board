using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public readonly struct ClearPiecesCommand : IBoardCommand, IDisposable
    {
        private readonly List<IPiece> piecesToClear;
        private readonly PiecesClearManager piecesClearManager;

        public ClearPiecesCommand(List<IPiece> piecesToClear, PiecesClearManager piecesClearManager)
        {
#if UNITY_2022_1_OR_NEWER
            this.piecesToClear = UnityEngine.Pool.ListPool<IPiece>.Get();
            this.piecesToClear.AddRange(piecesToClear);
#else
            this.piecesToClear = piecesToClear;
#endif
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

        private readonly bool IsClearing() => piecesClearManager.IsClearing;

        public override readonly string ToString()
        {
            return $"Command to clear {piecesToClear.Count} pieces";
        }

        public void Dispose()
        {
#if UNITY_2022_1_OR_NEWER
            UnityEngine.Pool.ListPool<IPiece>.Release(piecesToClear);
#endif
        }
    }
}
