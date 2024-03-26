using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public abstract class PieceTypeProvider : MonoBehaviour
    {
        public abstract IPieceType GetPieceType(int x, int y);
    }
}
