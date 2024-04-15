using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PiecesColorProvider : MonoBehaviour
    {
        public abstract IPieceColor GetPieceColor(int x, int y);
    }
}
