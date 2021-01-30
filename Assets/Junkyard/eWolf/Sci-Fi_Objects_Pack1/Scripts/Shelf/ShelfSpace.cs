using System.Collections.Generic;
using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    public class ShelfSpace
    {
        private List<ShelfSpaceDetails> _shelfSpaceDetails = new List<ShelfSpaceDetails>();

        public void DebugDisplay()
        {
            Debug.Log("---------------------------");
            foreach (var details in _shelfSpaceDetails)
            {
                Debug.Log($"{details.Start} {details.End}");
            }
        }

        public float GetPosition(ShelfObjects item)
        {
            float d = Random.Range(0, 100);
            d /= 100;

            float start = d - (item.Size / 2);
            if (start < 0)
            {
                d = item.Size / 2;
                start = d - (item.Size / 2);
            }
            float end = d + (item.Size / 2);
            if (end > 1)
            {
                d = 1 - item.Size / 2;
                end = d + (item.Size / 2);
            }

            ShelfSpaceDetails shelfSpace = new ShelfSpaceDetails()
            {
                Start = start,
                End = end
            };

            if (!AnyOverlaps(shelfSpace))
            {
                _shelfSpaceDetails.Add(shelfSpace);
                return d;
            }

            return 0;
        }

        private bool AnyOverlaps(ShelfSpaceDetails shelfSpace)
        {
            foreach (var details in _shelfSpaceDetails)
            {
                if (shelfSpace.Start < details.End
                    && shelfSpace.End > details.Start)
                {
                    return true;
                }
            }
            return false;
        }
    }
}