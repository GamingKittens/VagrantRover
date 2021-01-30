using UnityEngine;

namespace eWolf.SciFiObjects
{
    public static class PerfabBuilder
    {
        public static GameObject CreatePrefab(Vector3 position, string prefabName)
        {
            GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(prefabName),
                position,
                Quaternion.identity);

            return go;
        }

        public static void RemoveCollision(GameObject gameObject)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    var col = gameObject.GetComponent<BoxCollider>();
                    GameObject.DestroyImmediate(col, true);
                }
                catch { }

                try
                {
                    var meshCol = gameObject.GetComponent<MeshCollider>();
                    GameObject.DestroyImmediate(meshCol, true);
                }
                catch { }
            }
        }
    }
}