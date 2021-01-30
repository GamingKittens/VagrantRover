using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scales the object vertically based on the distance between two points.
public class ScaleBetween : MonoBehaviour
{
    public Transform target;
    public Transform target2;

    public bool runOnUpdate = true;
    public int factor = 1;

    private void Start()
    {
        if (!runOnUpdate)
            Destroy(this);
    }

    void Update()
    {
        Vector3 newScale = transform.localScale;
        newScale.y = Vector3.Distance(target.position, target2.position) * factor;
        transform.localScale = newScale;
    }

    [NaughtyAttributes.Button]
    void RunOnce()
    {
        Update();
    }
}