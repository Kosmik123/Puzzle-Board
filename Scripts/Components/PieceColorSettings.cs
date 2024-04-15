using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Piece Color Settings")]
    public class PieceColorSettings : PieceColorsList
    {
        [SerializeField]
        private PieceColor[] possiblePieceColors;

        public override int Count => possiblePieceColors.Length;

        public override IPieceColor this[int index] => possiblePieceColors[index];

        public override IEnumerator<IPieceColor> GetEnumerator() => (IEnumerator<IPieceColor>)possiblePieceColors.GetEnumerator();
    }
}
