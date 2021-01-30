using eWolf.SciFiObjects.Shelf;
using UnityEditor;
using UnityEngine;

namespace eWolf.SciFiObjects
{
    [CustomEditor(typeof(ShelfObjectJar))]
    public class ShelfObjectJar_UI : Editor
    {
        private ShelfObjectJar _node;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_node.IsLocked)
            {
                GUILayout.Label("* Unlocked to randomize objects");
                return;
            }

            if (GUILayout.Button("Randomize level"))
            {
                _node.SetLevel();
                EditorUtility.SetDirty(target);
            }
        }

        private void OnEnable()
        {
            _node = (ShelfObjectJar)target;
        }
    }
}
