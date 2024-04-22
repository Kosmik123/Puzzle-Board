using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PiecesSwapManager : MonoBehaviour
    {
        [SerializeField]
        private BoardComponent boardComponent;

        public void SwapPieces(Piece piece1, Piece piece2, Vector2Int targetCoord1, Vector2Int targetCoord2)
        {
            var pieceComponent1 = boardComponent.GetPieceComponent(piece1);
            var pieceComponent2 = boardComponent.GetPieceComponent(piece2);

            pieceComponent1.MoveTo(boardComponent.CoordToWorld(targetCoord1));
            pieceComponent2.MoveTo(boardComponent.CoordToWorld(targetCoord2));
        }
    }
}
