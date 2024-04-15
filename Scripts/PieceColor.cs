using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Piece Color")]
    public class PieceColor : ScriptableObject, IPieceColor
    {
        public override string ToString() => name;
    }
}
