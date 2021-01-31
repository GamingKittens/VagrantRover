using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    public WheelCollider[] wheels;
    public float brakeTorque = 300;
    public float maxVelocity = 6;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogWarning("Couldn't find rigidboy for menu player");

        foreach (WheelCollider wc in wheels)
            wc.brakeTorque = brakeTorque;
    }

    void FixedUpdate ()
    {
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = rb.velocity.normalized * maxVelocity;
    }
}