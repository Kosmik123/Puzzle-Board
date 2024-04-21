using Bipolar.PuzzleBoard.Components;
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
        private readonly IBoardComponent boardComponent;

        public SwapPiecesCommand(Piece piece1, Piece piece2, Vector2Int targetCoord1, Vector2Int targetCoord2, IBoardComponent boardComponent)
        {
            this.piece1 = piece1;
            this.piece2 = piece2;
            this.targetCoord1 = targetCoord1;
            this.targetCoord2 = targetCoord2;
            this.boardComponent = boardComponent;
        }

        public IEnumerator Execute()
        {
            var pieceComponent1 = boardComponent.GetPieceComponent(piece1);
            var pieceComponent2 = boardComponent.GetPieceComponent(piece2);

            pieceComponent1.MoveTo(boardComponent.CoordToWorld(targetCoord1));
            pieceComponent2.MoveTo(boardComponent.CoordToWorld(targetCoord2));

            yield return new WaitForSeconds(0.3f);
        }
    }
}
