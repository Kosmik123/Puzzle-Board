using MackySoft.SerializeReferenceExtensions.Editor;
using UnityEditor;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Editor
{
    public class PiecePropertyDrawer : PropertyDrawer
    {
        protected static void DrawPropertyName(Rect position, SerializedProperty property)
        {
            string name = property?.boxedValue?.GetType()?.Name;
            EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(name));
        }
    }

    [CustomPropertyDrawer(typeof(IPieceProperty), useForChildren: true)]
    public class StaticPiecePropertyDrawer : PiecePropertyDrawer
    {


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawPropertyName(position, property);
        }
    }

    //[CustomPropertyDrawer(typeof(DynamicPieceProperty), useForChildren: true)]
    public class DynamicPiecePropertyDrawer : PiecePropertyDrawer
    {



        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawPropertyName(position, property);

            Rect childPosition = position;
            float height = EditorGUIUtility.singleLineHeight;
            foreach (var childProperty in property.GetChildProperties())
            {
                childPosition.y += height + EditorGUIUtility.standardVerticalSpacing;
                height = EditorGUI.GetPropertyHeight(childProperty, new GUIContent(childProperty.displayName, childProperty.tooltip), true);
                childPosition.height = height;
                EditorGUI.PropertyField(childPosition, childProperty, true);
            }
        }
    }
}