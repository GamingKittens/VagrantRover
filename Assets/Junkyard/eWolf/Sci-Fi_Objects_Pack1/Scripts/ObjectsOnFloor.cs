using eWolf.Common.Helper;
using eWolf.Common.Interfaces;
using eWolf.SciFiObjects.Shelf;
using System.Collections.Generic;
using UnityEngine;

namespace eWolf.SciFiObjects
{
    public class ObjectsOnFloor : MonoBehaviour, IRandomizer
    {
        public int Count = 8;
        public bool Locked = false;
        public RandomMaterialHolder RandomMaterialHolder = new RandomMaterialHolder();
        public ShelfObjectCollections ShelfObjectCollection = new ShelfObjectCollections();
        public float Size = 0.25f;

        public bool IsLocked
        {
            get
            {
                return Locked;
            }
        }

        public void ClearObjects()
        {
            ObjectHelper.RemoveAllObjectFrom(gameObject, null);
        }

        public void DropObjects()
        {
            ObjectHelper.RemoveAllObjectFrom(gameObject, null);

            int count = UnityEngine.Random.Range(Count / 2, Count);

            var collection = ShelfObjectCollection.GetObjects();
            if (collection.Count == 0)
                return;

            List<ObjectOnFloorSpace> spaces = new List<ObjectOnFloorSpace>();

            float sizeScale = Size * 2000;
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = transform.position;
                float x = UnityEngine.Random.Range(-sizeScale, sizeScale);
                x /= 2000;
                float y = UnityEngine.Random.Range(-sizeScale, sizeScale);
                y /= 2000;
                pos.x += x;
                pos.z += y;

                int itemIndex = UnityEngine.Random.Range(0, collection.Count);
                ShelfObjects item = collection[itemIndex];

                ObjectOnFloorSpace objectOnFloorSpace = new ObjectOnFloorSpace()
                {
                    Position = pos,
                    Size = item.Size
                };

                if (!IsSpaceClear(spaces, objectOnFloorSpace))
                {
                    continue;
                }

                string prefabName = item.Name;
                var go = PerfabBuilder.CreatePrefab(pos, prefabName);

                go.transform.parent = gameObject.transform;

                go.transform.rotation = new Quaternion();
                float angle = UnityEngine.Random.Range(0, 360);
                go.transform.Rotate(0, 0, angle);
                RandomMaterialHolder.ReplaceMaterials(go);
                spaces.Add(objectOnFloorSpace);
            }
        }

        public void OnDrawGizmosSelected()
        {
            Vector3 pos = transform.position;
            float sizeScale = Size;
            pos.x -= sizeScale;
            pos.z -= sizeScale;
            Vector3 pos2 = pos;
            pos2.x += sizeScale * 2;

            Vector3 posB = pos;
            Vector3 pos2B = pos2;
            posB.z += sizeScale * 2;
            pos2B.z += sizeScale * 2;

            Debug.DrawLine(pos, pos2);
            Debug.DrawLine(posB, pos2B);
            Debug.DrawLine(pos, posB);
            Debug.DrawLine(pos2, pos2B);
        }

        public void Randomize()
        {
            Debug.Log($"Randomizing shelf {gameObject.name}");
            DropObjects();
        }

        public void RandomizeVisual()
        {
            var objects = ObjectHelper.GetAllChildObjects(gameObject);
            foreach (var obj in objects)
            {
                ShelfObjectBase sob = obj.GetComponent<ShelfObjectBase>();
                if (sob != null)
                {
                    RandomMaterialHolder.ReplaceMaterials(obj);
                }
            }
        }

        private bool IsSpaceClear(List<ObjectOnFloorSpace> spaces, ObjectOnFloorSpace objectOnFloorSpace)
        {
            foreach (var space in spaces)
            {
                var diff = space.Position - objectOnFloorSpace.Position;
                var mag = diff.magnitude;

                if (mag < (space.Size + objectOnFloorSpace.Size))
                {
                    return false;
                }
            }
            return true;
        }

        private class ObjectOnFloorSpace
        {
            public Vector3 Position;
            public float Size;
        }
    }
}
