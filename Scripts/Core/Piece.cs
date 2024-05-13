using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    [System.Serializable]
    public class Piece
    {
        public event System.Action OnCleared;

        [SerializeField]
        private bool isCleared = false;
        public bool IsCleared => isCleared;

        // private IBoard containingBoard;

        [SerializeReference]
        public List<PieceProperty> pieceProperties = new List<PieceProperty>();

        public static bool Exists(Piece piece) => piece != null && !piece.IsCleared;
        public virtual IPieceColor Color { get; set; }

        public Piece (IPieceColor color)
        {
            Color = color;
        }

        public void ClearPiece()
        {
            isCleared = true;
            OnCleared?.Invoke();
        }

#if UNITY_EDITOR
        internal void Validate()
        {
            Color = Color;
        }
#endif
        public override string ToString()
        {
            return $"Piece ({Color})";
        }

        public bool HasProperty<T>() where T : PieceProperty
        {
            return TryGetProperty<T>(out _);
        }

        public bool TryGetProperty<T>(out T pieceProperty) where T : PieceProperty
        {
            pieceProperty = null;
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

        public bool RemoveProperty<T>() where T : PieceProperty
        {
            for (int i = pieceProperties.Count - 1; i >= 0; i--)
            {
                if (pieceProperties[i] is T)
                {
                    pieceProperties.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveProperty(PieceProperty property) => pieceProperties.Remove(property);

        public void AddProperty(PieceProperty property)
        {
            pieceProperties.Add(property);
        }

        public void AddProperty<T>() where T : StaticPieceProperty
        {
            pieceProperties.Add(StaticPieceProperty.Get<T>());
        }
    }

    public class Piece<T> : Piece
        where T : Object, IPieceColor
    {
        [SerializeField]
        private T color;

        public Piece(IPieceColor color) : base(color)
        {
            AddProperty<ImmovablePieceProperty>();
            AddProperty(new FrozenPieceProperty());
        }

        public override IPieceColor Color
        {
            get => color;
            set
            {
                color = value as T;
                base.Color = color;
            }
        }
    }

    [System.Serializable]
    public abstract class PieceProperty
    {

    }

    [System.Serializable]
    public class FrozenPieceProperty : PieceProperty
    {
        public int hitPoints;
    }
    
    [System.Serializable]
    public class ImmovablePieceProperty : StaticPieceProperty
    { }

    [System.Serializable]
    public abstract class StaticPieceProperty : PieceProperty
    {
        private static readonly Dictionary<System.Type, StaticPieceProperty> instances = new Dictionary<System.Type, StaticPieceProperty>();

        public string Name;

        public static T Get<T>() where T : StaticPieceProperty
        {
            if (instances.TryGetValue(typeof(T), out var instance) == false)
            {
                instance = System.Activator.CreateInstance<T>();
                instances.Add(typeof(T), instance);
            }

            return (T)instance;
        }

        private protected StaticPieceProperty()
        { }
    }
}
