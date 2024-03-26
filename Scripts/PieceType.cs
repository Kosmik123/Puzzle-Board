using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceType
    { }

    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Piece Type")]
    public class PieceType : ScriptableObject, IPieceType
    { }
}
