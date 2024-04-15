using UnityEditor;
using UnityEngine;

namespace Bipolar.PuzzleBoard.Editor
{
    [CustomEditor(typeof(BoardCollapseController<,>), editorForChildClasses: true)]
    public class BoardCollapseControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


        }
    }
}
