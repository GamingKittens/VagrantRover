using UnityEditor;
using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    [CustomEditor(typeof(ScifiObjectBase))]
    public class ScifiObjectBase_UI : Editor
    {
        private ScifiObjectBase _node;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUI.color = Color.green;
            if (_node.IsLocked)
            {
                GUILayout.Label("* Unlocked to randomize objects");
                return;
            }
            if (GUILayout.Button("Randomize Colour"))
            {
                _node.RandomizeVisual();
                EditorUtility.SetDirty(target);
            }
        }

        private void OnEnable()
        {
            _node = (ScifiObjectBase)target;
        }
    }
}
