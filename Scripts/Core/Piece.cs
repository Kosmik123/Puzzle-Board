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

    public class Piece<TColor> : Piece
        where TColor : IPieceColor
    {
        [SerializeField]
        private TColor color;

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
                color = (TColor)value;
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

        [SerializeField]
        private string Name;

        public static T Get<T>() where T : StaticPieceProperty
        {
            var propertyType = typeof(T);
            if (instances.TryGetValue(propertyType, out var instance) == false)
            {
                instance = System.Activator.CreateInstance<T>();
                instance.Name = propertyType.Name;
                instances.Add(propertyType, instance);
            }

            return (T)instance;
        }

        protected StaticPieceProperty()
        { }
    }
#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(StaticPieceProperty), useForChildren: true)]
    public class StaticPiecePropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            var name = property.FindPropertyRelative("Name").stringValue;
            UnityEditor.EditorGUI.LabelField(position, UnityEditor.ObjectNames.NicifyVariableName(name));
        }
    }
#endif
}
