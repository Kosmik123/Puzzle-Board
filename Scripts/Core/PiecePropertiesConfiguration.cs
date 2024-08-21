using System;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IPiecePropertiesConfiguration
    {
        void ConfigureProperties(ICompoundPiece piece, Vector2Int coord);
    }

    [CreateAssetMenu(menuName = CreateAssetsPath.Root + "Piece Properties Configuration")]
    public class PiecePropertiesConfiguration : ScriptableObject, IPiecePropertiesConfiguration
    {
        [SerializeReference, SubclassSelector]
        private IPieceProperty[] properties;

        public void ConfigureProperties(ICompoundPiece piece, Vector2Int coord)
        {
            foreach (var property in properties)
            {
                piece.AddProperty(property);
            }
        }
    }
}