using eWolf.Common.Interfaces;
using eWolf.SciFiObjects.Interfaces;
using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    public class ShelfObjectJar : MonoBehaviour, IShelfObject, IRandomizer
    {
        public bool Locked = false;
        public bool RandomLevels = true;
        private GameObject _insideJar;

        public bool IsLocked
        {
            get
            {
                return Locked;
            }
        }

        public void InitShelfObject()
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == "InsideJar")
                {
                    _insideJar = child.gameObject;
                }
            }

            if (RandomLevels)
            {
                SetLevel();
            }
        }

        public void Randomize()
        {
            SetLevel();
        }

        public void RandomizeVisual()
        {
            SetLevel();
        }

        public void SetLevel()
        {
            if (_insideJar == null)
                InitShelfObject();

            float level = Random.Range(1, 1000);
            level /= 1000;
            if (level < 0.05f)
            {
                _insideJar.SetActive(false);
                return;
            }

            _insideJar.SetActive(true);
            var scale = _insideJar.transform.localScale;
            scale.z = level;
            _insideJar.transform.localScale = scale;
        }

        public void Start()
        {
        }
    }
}