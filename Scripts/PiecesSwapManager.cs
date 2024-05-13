using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class PiecesSwapManager : MonoBehaviour
    {
        [SerializeField]
        private SceneBoard sceneBoard;

        public void SwapPieces(Piece piece1, Piece piece2, Vector2Int targetCoord1, Vector2Int targetCoord2)
        {
            var scenePiece1 = sceneBoard.GetScenePiece(piece1);
            var scenePiece2 = sceneBoard.GetScenePiece(piece2);

            scenePiece1.MoveTo(sceneBoard.CoordToWorld(targetCoord1));
            scenePiece2.MoveTo(sceneBoard.CoordToWorld(targetCoord2));
        }
    }
}
