using UnityEditor;
using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    [CustomEditor(typeof(ObjectsOnFloor))]
    public class ObjectsOnFloor_UI : Editor
    {
        private ObjectsOnFloor _node;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_node.IsLocked)
            {
                GUILayout.Label("* Unlocked to randomize objects");
                return;
            }

            GUI.color = Color.green;
            if (GUILayout.Button("Clear Objects"))
            {
                _node.ClearObjects();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Drop objects"))
            {
                _node.DropObjects();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Randomize Colour"))
            {
                _node.RandomizeVisual();
                EditorUtility.SetDirty(target);
            }
        }

        private void OnEnable()
        {
            _node = (ObjectsOnFloor)target;
        }
    }
}
