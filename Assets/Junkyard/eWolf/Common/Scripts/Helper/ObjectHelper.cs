using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by Sci-Fi_Objects_Pack1
/// </summary>
namespace eWolf.Common.Helper
{
    public static class ObjectHelper
    {
        public static GameObject FindChildObject(GameObject baseObject, string name)
        {
            GameObject gameObject = baseObject;
            if (!string.IsNullOrWhiteSpace(name))
            {
                foreach (Transform child in baseObject.transform)
                {
                    if (child.name == name)
                    {
                        gameObject = child.gameObject;
                        break;
                    }
                }
            }
            return gameObject;
        }

        public static List<GameObject> GetAllChildObjects(GameObject baseObject, string name)
        {
            var go = FindChildObject(baseObject, name);
            return GetAllChildObjects(go);
        }

        public static List<GameObject> GetAllChildObjects(GameObject baseObject)
        {
            List<GameObject> objects = new List<GameObject>();

            foreach (Transform child in baseObject.transform)
            {
                objects.Add(child.gameObject);
            }
            return objects;
        }

        public static void RemoveAllObjectFrom(GameObject baseObject, string name)
        {
            var gameObject = FindChildObject(baseObject, name);

            var objects = GetAllChildObjects(gameObject);

            for (int i = 0; i < objects.Count; i++)
            {
                GameObject.DestroyImmediate(objects[i]);
            }
        }
    }
}
