using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PieceColorProvider : MonoBehaviour, IPieceColorProvider
    {
        public abstract IPieceColor GetPieceColor(int x, int y);
    }
}
