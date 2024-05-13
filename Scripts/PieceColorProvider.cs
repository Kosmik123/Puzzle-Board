using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceColorProvider
    {
        IPieceColor GetPieceColor(int x, int y);
    }

    public abstract class PieceColorProvider : MonoBehaviour, IPieceColorProvider
    {
        public abstract IPieceColor GetPieceColor(int x, int y);
    }
}
