using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceTypeProvider
    {
        public IPieceColor GetPieceType();
    }

    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Settings")]
    public class Settings : ScriptableObject, IPieceTypeProvider
    {
        [SerializeField]
        private PieceColor[] possiblePieceTypes;
        public IReadOnlyList<IPieceColor> PieceTypes => possiblePieceTypes;

        public IPieceColor GetPieceType()
        {
            return PieceTypes[Random.Range(0, PieceTypes.Count)];
        }

        public IPieceColor GetPieceTypeExcept(IPieceColor exception)
        {
            int index = Random.Range(1, PieceTypes.Count);
            if (PieceTypes[index] == exception)
                return PieceTypes[0];
            
            return PieceTypes[index];
        }

        private readonly List<IPieceColor> tempAvailableTypes = new List<IPieceColor>();
        public IPieceColor GetPieceTypeExcept(IEnumerable<IPieceColor> exceptions)
        {
            tempAvailableTypes.Clear();
            foreach (var type in PieceTypes)
                if (exceptions.Contains(type) == false)
                    tempAvailableTypes.Add(type);

            if (tempAvailableTypes.Count <= 0)
                return GetPieceType();

            return tempAvailableTypes[Random.Range(0, tempAvailableTypes.Count)];
        }
    }
}
