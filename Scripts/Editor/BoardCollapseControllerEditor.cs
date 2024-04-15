using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Editor
{
    // [CustomEditor(typeof(BoardCollapseController<,>), editorForChildClasses: true)]
    public class BoardCollapseControllerEditor : UnityEditor.Editor
    {
        private static readonly GUIContent StrategyTypeFieldLabel = new GUIContent("Strategy Type");
        private static readonly Type baseGenericStrategyType = typeof(BoardCollapseStrategy<>);
        
        protected class TypeGUIContent : GUIContent
        {
            public Type Type { get; private set; }

            public TypeGUIContent(Type type) : this(type.Name, type)
            { }

            public TypeGUIContent(string label, Type type) : base(label)
            {
                Type = type;
            }
        }

        private List<Type> compatibleTypes;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var boardType = ((BoardCollapseController)serializedObject.targetObject).BoardType;
            var compatibleStrategiesTypes = GetCompatibleStrategiesTypes(boardType);

            var compatibleStrategiesTypesNames = new string[compatibleStrategiesTypes.Count + 1];
            compatibleStrategiesTypesNames[0] = "None";
            for (int i = 0; i < compatibleStrategiesTypes.Count; i++)
                compatibleStrategiesTypesNames[i + 1] = compatibleStrategiesTypes[i].Name;

            var strategyProperty = serializedObject.FindProperty("strategy");
            var strategyTypeName = strategyProperty.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(strategyTypeName) == false)
            {
                var lastDot = strategyTypeName.LastIndexOf('.');
                strategyTypeName = strategyTypeName.Substring(lastDot + 1);
            }

            int previousOptionIndex = Array.FindIndex(compatibleStrategiesTypesNames, (name) => name == strategyTypeName);
            if (previousOptionIndex == -1)
                previousOptionIndex = 0;

            int optionIndex = EditorGUILayout.Popup(StrategyTypeFieldLabel, previousOptionIndex, compatibleStrategiesTypesNames);
            if (optionIndex != previousOptionIndex)
            {
                if (optionIndex == 0)
                {
                    strategyProperty.managedReferenceValue = null;
                }
                else
                {
                    var newStrategyType = compatibleStrategiesTypes[optionIndex - 1];
                    strategyProperty.managedReferenceValue = Activator.CreateInstance(newStrategyType);
                }
                serializedObject.ApplyModifiedProperties();
            }

            if (optionIndex > 0)
            {
                EditorGUILayout.PropertyField(strategyProperty, true);
            } 
        }

        protected IList<Type> GetCompatibleStrategiesTypes(Type boardType)
        {
            var baseStrategyType = baseGenericStrategyType.MakeGenericType(boardType);
            return TypeCache.GetTypesDerivedFrom(baseStrategyType);
        }
    }
}
