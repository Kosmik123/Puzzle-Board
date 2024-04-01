using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Bipolar.PuzzleBoard
{
    public class CollapseDirectionAttribute : PropertyAttribute
    { }

    [CustomPropertyDrawer(typeof(CollapseDirectionAttribute))]
    public class CollapseDirectionAttributeDrawer : PropertyDrawer
    {
        public class DirectionGUIContent : GUIContent
        {
            public Vector2Int Direction { get; private set; }

            public DirectionGUIContent(string text, Vector2Int direction) : base(text)
            {
                Direction = direction;
            }
        }

        private readonly DirectionGUIContent left = new DirectionGUIContent("Left", Vector2Int.left);
        private readonly DirectionGUIContent up = new DirectionGUIContent("Up", Vector2Int.up);
        private readonly DirectionGUIContent right = new DirectionGUIContent("Right", Vector2Int.right);
        private readonly DirectionGUIContent down = new DirectionGUIContent("Down", Vector2Int.down);
        private readonly DirectionGUIContent upLeft = new DirectionGUIContent("Up Left", Vector2Int.up + Vector2Int.left);
        private readonly DirectionGUIContent upRight = new DirectionGUIContent("Up Right", Vector2Int.up + Vector2Int.right);
        private readonly DirectionGUIContent downLeft = new DirectionGUIContent("Down Left", Vector2Int.down + Vector2Int.left);
        private readonly DirectionGUIContent downRight = new DirectionGUIContent("Down Right", Vector2Int.down + Vector2Int.right);

        private readonly DirectionGUIContent upLeftIsometric = new DirectionGUIContent("Up Left", Vector2Int.up);
        private readonly DirectionGUIContent upRightIsometric= new DirectionGUIContent("Up Right", Vector2Int.right);
        private readonly DirectionGUIContent downRightIsometric = new DirectionGUIContent("Down Right", Vector2Int.down);
        private readonly DirectionGUIContent downLeftIsometric = new DirectionGUIContent("Down Left", Vector2Int.left);

        private readonly List<DirectionGUIContent> optionsList = new List<DirectionGUIContent>();
        private DirectionGUIContent[] options;
        private GridLayout.CellLayout layout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (TryDrawCustomProperty(position, property, label) == false)
                EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }

        private bool TryDrawCustomProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector2Int)
                return false;

            var collapsing = property.serializedObject.targetObject;
            var collapsingType = collapsing.GetType();
            var boardPropertyInfo = collapsingType.GetProperty("Board", BindingFlags.Instance | BindingFlags.Public);
            var board = boardPropertyInfo.GetValue(collapsing) as Board;
            if (board == null)
                return false;

            var boardLayout = board.Layout;
            if (options == null || options.Length <= 0 || layout != boardLayout)
            {
                layout = boardLayout;
                optionsList.Clear();
                switch (boardLayout)
                {
                    case GridLayout.CellLayout.Rectangle:
                        optionsList.Add(left);
                        optionsList.Add(up);
                        optionsList.Add(right);
                        optionsList.Add(down);
                        break;

                    case GridLayout.CellLayout.Hexagon:
                        optionsList.Add(left);
                        optionsList.Add(upLeft);
                        optionsList.Add(upRight);
                        optionsList.Add(right);
                        optionsList.Add(downRight);
                        optionsList.Add(downLeft);
                        break;

                    case GridLayout.CellLayout.Isometric:
                    case GridLayout.CellLayout.IsometricZAsY:
                        optionsList.Add(upLeftIsometric);
                        optionsList.Add(upRightIsometric);
                        optionsList.Add(downRightIsometric);
                        optionsList.Add(downLeftIsometric);
                        break;
                }
                options = optionsList.ToArray();
            }

            var direction = property.vector2IntValue;
            int index = optionsList.FindIndex(content => content.Direction == direction);
            int newSelectedDirectionIndex = EditorGUI.Popup(position, label, Mathf.Max(index, 0), options);
            if (newSelectedDirectionIndex != index)
            {
                direction = options[newSelectedDirectionIndex].Direction;
                property.vector2IntValue = direction;
                property.serializedObject.ApplyModifiedProperties();
            }
            return true;
        }
    }
}
