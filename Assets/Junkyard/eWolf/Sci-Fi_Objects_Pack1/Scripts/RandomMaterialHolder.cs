using eWolf.SciFiObjects.Shelf;
using System.Collections.Generic;
using UnityEngine;

namespace eWolf.SciFiObjects
{
    [System.Serializable]
    public class RandomMaterialHolder
    {
        public List<Material> Materials = new List<Material>();

        public void ReplaceMaterials(GameObject shelfObject)
        {
            var shelfObjectBase = shelfObject.GetComponent<ShelfObjectBase>();
            if (shelfObjectBase != null)
            {
                if (!shelfObjectBase.AllowMaterialReplace)
                    return;
            }

            if (Materials.Count == 0)
            {
                Debug.LogError($"{shelfObject.name}: Don't have any materials set in the material list to randomize. [RandomMaterialHolder]");
                return;
            }

            int materialIndex = Random.Range(0, Materials.Count);
            var renderer = shelfObject.GetComponent<Renderer>();
            renderer.sharedMaterial = Materials[materialIndex];
        }
    }
}
