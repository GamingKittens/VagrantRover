using eWolf.Common.Helper;
using eWolf.Common.Interfaces;
using eWolf.SciFiObjects.Interfaces;
using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    public class ShelfFiller : MonoBehaviour, IRandomizer
    {
        public int LevelsOnShelf = 4;
        public bool Locked = false;
        public RandomMaterialHolder RandomMaterialHolder = new RandomMaterialHolder();
        public ShelfCapacities ShelfCapacity = ShelfCapacities.Normal;
        public ShelfCollisionStyles ShelfcollisionStyle = ShelfCollisionStyles.Full;
        public ShelfObjectCollections ShelfObjectCollection = new ShelfObjectCollections();

        public bool IsLocked
        {
            get
            {
                return Locked;
            }
        }

        public void ClearShelf()
        {
            ObjectHelper.RemoveAllObjectFrom(gameObject, "Level1");
            if (LevelsOnShelf > 1)
                ObjectHelper.RemoveAllObjectFrom(gameObject, "Level2");
            if (LevelsOnShelf > 2)
                ObjectHelper.RemoveAllObjectFrom(gameObject, "Level3");
            if (LevelsOnShelf > 3)
                ObjectHelper.RemoveAllObjectFrom(gameObject, "Level4");
        }

        public void ClearShelf(int level)
        {
            string name = $"Level{level}";
            ObjectHelper.RemoveAllObjectFrom(gameObject, name);
        }

        public void FillShelf()
        {
            PopulateLevel(1);
            PopulateLevel(2);
            PopulateLevel(3);
            PopulateLevel(4);
        }

        public void Randomize()
        {
            Debug.Log($"Randomizing shelf {gameObject.name}");
            FillShelf();
        }

        public void RandomizeVisual()
        {
            RandomizeColour();
        }

        public void ShelfLevel1()
        {
            PopulateLevel(1);
        }

        public void ShelfLevel2()
        {
            PopulateLevel(2);
        }

        public void ShelfLevel3()
        {
            PopulateLevel(3);
        }

        public void ShelfLevel4()
        {
            PopulateLevel(4);
        }

        private void CreateObjectOnShelf(int level, ShelfDetails shelfDetails, ShelfObjects item, float positionOnShelf)
        {
            Vector3 pos = shelfDetails.GetPosition(positionOnShelf, level);

            string prefabName = item.Name;
            GameObject go = PerfabBuilder.CreatePrefab(pos, prefabName);
            if (ShelfcollisionStyle == ShelfCollisionStyles.Quick)
            {
                PerfabBuilder.RemoveCollision(go);
            }
            go.transform.parent = shelfDetails.ParentObject(level).transform;

            go.transform.rotation = new Quaternion();
            float angle = Random.Range(0, 360);
            if (item.Rotate)
            {
                go.transform.Rotate(angle, 90, 0);
            }
            else
            {
                go.transform.Rotate(0, 0, angle);
            }

            RandomMaterialHolder.ReplaceMaterials(go);
            InitObjects(go);
        }

        private void InitObjects(GameObject shelfObject)
        {
            IShelfObject init = shelfObject.GetComponent<IShelfObject>();
            if (init != null)
            {
                init.InitShelfObject();
            }
        }

        private void PopulateLevel(int level)
        {
            if (level > LevelsOnShelf)
                return;

            ClearShelf(level);

            var collection = ShelfObjectCollection.GetObjects();
            if (collection.Count == 0)
                return;

            ShelfDetails shelfDetails = new ShelfDetails(this.gameObject);
            ShelfSpace shelfSpaceLevel = new ShelfSpace();

            int count = Random.Range(1, 6);
            if (ShelfCapacity == ShelfCapacities.Filled)
            {
                count += 15;
            }

            for (int i = 0; i < count; i++)
            {
                int itemIndex = Random.Range(0, collection.Count);
                var item = collection[itemIndex];

                float positionOnShelf = shelfSpaceLevel.GetPosition(item);
                if (positionOnShelf != 0)
                {
                    CreateObjectOnShelf(level, shelfDetails, item, positionOnShelf);
                }
            }
        }

        private void RandomizeColour()
        {
            RandomizeColour(1);
            RandomizeColour(2);
            RandomizeColour(3);
            RandomizeColour(4);
        }

        private void RandomizeColour(int level)
        {
            if (level > LevelsOnShelf)
                return;

            string name = $"Level{level}";

            var objects = ObjectHelper.GetAllChildObjects(gameObject, name);
            foreach (var obj in objects)
            {
                ShelfObjectBase sob = obj.GetComponent<ShelfObjectBase>();
                if (sob != null)
                {
                    RandomMaterialHolder.ReplaceMaterials(obj);
                }
            }
        }
    }
}