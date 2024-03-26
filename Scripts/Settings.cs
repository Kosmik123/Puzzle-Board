using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bipolar.PuzzleBoard
{
    public interface IPieceTypeProvider
    {
        public IPieceType GetPieceType();
    }

    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Settings")]
    public class Settings : ScriptableObject, IPieceTypeProvider
    {
        [SerializeField]
        private PieceType[] possiblePieceTypes;
        public IReadOnlyList<IPieceType> PieceTypes => possiblePieceTypes;

        public IPieceType GetPieceType()
        {
            return PieceTypes[Random.Range(0, PieceTypes.Count)];
        }

        public IPieceType GetPieceTypeExcept(IPieceType exception)
        {
            int index = Random.Range(1, PieceTypes.Count);
            if (PieceTypes[index] == exception)
                return PieceTypes[0];
            
            return PieceTypes[index];
        }

        private readonly List<IPieceType> tempAvailableTypes = new List<IPieceType>();
        public IPieceType GetPieceTypeExcept(IEnumerable<IPieceType> exceptions)
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
