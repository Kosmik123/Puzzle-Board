using System.Collections;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public struct SwapPiecesCommand : IBoardCommand
    {
        private readonly Piece piece1;
        private readonly Piece piece2;
        private readonly Vector2Int targetCoord1;
        private readonly Vector2Int targetCoord2;
        private readonly PiecesSwapManager piecesSwapManager;

        public SwapPiecesCommand(Piece piece1, Piece piece2, Vector2Int targetCoord1, Vector2Int targetCoord2, PiecesSwapManager piecesSwapManager)
        {
            this.piece1 = piece1;
            this.piece2 = piece2;
            this.targetCoord1 = targetCoord1;
            this.targetCoord2 = targetCoord2;
            this.piecesSwapManager = piecesSwapManager;
        }

        public IEnumerator Execute()
        {
            piecesSwapManager.SwapPieces(piece1, piece2, targetCoord1, targetCoord2);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
