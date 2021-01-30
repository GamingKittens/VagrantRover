using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour
{
    public Transform target;

    public bool runOnUpdate = true;

    private void Start()
    {
        if (!runOnUpdate)
            Destroy(this);
    }

    void Update()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
    }

    [NaughtyAttributes.Button]
    void RunOnce ()
    {
        Update();
    }
}