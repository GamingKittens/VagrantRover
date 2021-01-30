using UnityEditor;
using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    [CustomEditor(typeof(ShelfFiller))]
    public class ShelfFiller_UI : Editor
    {
        private ShelfFiller _node;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_node.IsLocked)
            {
                GUILayout.Label("* Unlocked to randomize shelf");
                return;
            }

            GUI.color = Color.green;
            if (GUILayout.Button("Clear Shelf"))
            {
                _node.ClearShelf();
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("Fill all Shelves"))
            {
                _node.FillShelf();
                EditorUtility.SetDirty(target);
            }
            GUI.color = Color.yellow;
            if (GUILayout.Button("Update Shelf 1"))
            {
                _node.ShelfLevel1();
                EditorUtility.SetDirty(target);
            }
            if (_node.LevelsOnShelf > 1)
                if (GUILayout.Button("Update Shelf 2"))
                {
                    _node.ShelfLevel2();
                    EditorUtility.SetDirty(target);
                }
            if (_node.LevelsOnShelf > 2)
                if (GUILayout.Button("Update Shelf 3"))
                {
                    _node.ShelfLevel3();
                    EditorUtility.SetDirty(target);
                }
            if (_node.LevelsOnShelf > 3)
                if (GUILayout.Button("Update Shelf 4"))
                {
                    _node.ShelfLevel4();
                    EditorUtility.SetDirty(target);
                }

            GUI.color = Color.cyan;
            if (GUILayout.Button("Clear Shelf 1"))
            {
                _node.ClearShelf(1);
                EditorUtility.SetDirty(target);
            }
            if (_node.LevelsOnShelf > 1)
                if (GUILayout.Button("Clear  Shelf 2"))
                {
                    _node.ClearShelf(2);
                    EditorUtility.SetDirty(target);
                }
            if (_node.LevelsOnShelf > 2)
                if (GUILayout.Button("Clear  Shelf 3"))
                {
                    _node.ClearShelf(3);
                    EditorUtility.SetDirty(target);
                }
            if (_node.LevelsOnShelf > 3)
                if (GUILayout.Button("Clear  Shelf 4"))
                {
                    _node.ClearShelf(4);
                    EditorUtility.SetDirty(target);
                }

            GUI.color = Color.green;
            if (GUILayout.Button("Randomize Colour"))
            {
                _node.RandomizeVisual();
                EditorUtility.SetDirty(target);
            }
        }

        private void OnEnable()
        {
            _node = (ShelfFiller)target;
        }
    }
}
