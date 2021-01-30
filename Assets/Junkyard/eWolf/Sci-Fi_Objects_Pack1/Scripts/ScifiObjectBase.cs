using eWolf.Common.Interfaces;
using UnityEngine;

namespace eWolf.SciFiObjects
{
    public class ScifiObjectBase : MonoBehaviour, IRandomizer
    {
        public bool Locked = false;
        public RandomMaterialHolder RandomMaterialHolder = new RandomMaterialHolder();

        public bool IsLocked
        {
            get
            {
                return Locked;
            }
        }

        public void Randomize()
        {
        }

        public void RandomizeVisual()
        {
            RandomMaterialHolder.ReplaceMaterials(gameObject);
        }
    }
}
