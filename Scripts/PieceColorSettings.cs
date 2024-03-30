using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bipolar.PuzzleBoard
{
    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Piece Color Settings")]
    public class PieceColorSettings : ScriptableObject
    {
        [SerializeField]
        private PieceColor[] possiblePieceColors;
        public IReadOnlyList<IPieceColor> PieceColors => possiblePieceColors;

        public IPieceColor GetPieceColor()
        {
            return PieceColors[Random.Range(0, PieceColors.Count)];
        }

        public IPieceColor GetPieceColorExcept(IPieceColor exception)
        {
            int index = Random.Range(1, PieceColors.Count);
            if (PieceColors[index] == exception)
                return PieceColors[0];
            
            return PieceColors[index];
        }

        private readonly List<IPieceColor> tempAvailableColors = new List<IPieceColor>();
        public IPieceColor GetPieceColorExcept(IEnumerable<IPieceColor> exceptions)
        {
            tempAvailableColors.Clear();
            foreach (var color in PieceColors)
                if (exceptions.Contains(color) == false)
                    tempAvailableColors.Add(color);

            if (tempAvailableColors.Count <= 0)
            {
                Debug.LogWarning("Couldn't find not adjacent piece color");
                return GetPieceColor();
            }

            return tempAvailableColors[Random.Range(0, tempAvailableColors.Count)];
        }
    }
}
