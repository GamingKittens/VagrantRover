using UnityEngine;

namespace eWolf.SciFiObjects.Shelf
{
    public class ShelfDetails
    {
        private Vector3 _left;
        private Vector3 _right;
        private Transform[] _levels = new Transform[5];

        public Vector3 GetPosition(float percentage, int level)
        {
            var pos = Vector3.Lerp(_left, _right, percentage);

            pos.y = _levels[level].position.y;

            float offSetAmount = Random.Range(0, 100);
            offSetAmount /= 100;
            offSetAmount -= 0.5f;

            Vector3 frontback = Vector3.Cross(Vector3.up, _left - _right);
            pos += frontback * (offSetAmount * 0.20f);

            return pos;
        }

        public Transform ParentObject(int level)
        {
            return _levels[level];
        }

        public ShelfDetails(GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == "Left")
                {
                    _left = child.gameObject.transform.position;
                }
                if (child.name == "Right")
                {
                    _right = child.gameObject.transform.position;
                }

                if (child.name == "Level1")
                {
                    _levels[1] = child.gameObject.transform;
                }
                if (child.name == "Level2")
                {
                    _levels[2] = child.gameObject.transform;
                }
                if (child.name == "Level3")
                {
                    _levels[3] = child.gameObject.transform;
                }
                if (child.name == "Level4")
                {
                    _levels[4] = child.gameObject.transform;
                }
            }
        }
    }
}