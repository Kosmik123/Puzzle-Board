using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public interface IReadOnlyCompoundPiece : IPiece
    {
        bool HasProperty<T>() where T : IPieceProperty;
        bool TryGetProperty<T>(out T pieceProperty) where T : IPieceProperty;
        public IReadOnlyList<IPieceProperty> Properties { get; }
    }

    public interface ICompoundPiece : IReadOnlyCompoundPiece
    {
        void AddProperty<T>() where T : IPieceProperty, new();
        void AddProperty(IPieceProperty pieceProperty);
        bool RemoveProperty<T>() where T : IPieceProperty;
        bool RemoveProperty(IPieceProperty property);
    }

    public class CompoundPiece : Piece, ICompoundPiece
    {
        [SerializeReference]
        private List<IPieceProperty> pieceProperties = new List<IPieceProperty>();
        public IReadOnlyList<IPieceProperty> Properties => pieceProperties;

        public bool HasProperty<T>() where T : IPieceProperty =>
            CompoundPieceHelper.HasProperty<T>(this);

        public bool TryGetProperty<T>(out T pieceProperty) where T : IPieceProperty
        {
            pieceProperty = default;
            foreach (var property in pieceProperties)
            {
                if (property is T typedProperty)
                {
                    pieceProperty = typedProperty;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveProperty<T>() where T : IPieceProperty
        {
            int index = -1;
            for (int i = 0; i < pieceProperties.Count; i++)
            {
                if (pieceProperties[i] is T)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return false;

            pieceProperties.RemoveAt(index);
            return true;
        }

        public bool RemoveProperty(IPieceProperty property) => pieceProperties.Remove(property);

        public void AddProperty(IPieceProperty property)
        {
            pieceProperties.Add(property);
        }

        public void AddProperty<T>() where T : IPieceProperty, new()
        {
            pieceProperties.Add(new T());
        }
    }

    public static class CompoundPieceHelper
    {
        public static bool HasProperty<T>(ICompoundPiece piece) where T : IPieceProperty
        {
            return piece.TryGetProperty<T>(out _);
        }


    }
}
