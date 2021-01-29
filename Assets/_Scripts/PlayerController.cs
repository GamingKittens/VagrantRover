using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public WheelCollider[] wheels;

    public float maxTorque = 200;
    public float maxBrake = 50;
    public float maxSpeed = 10; // (m/s)

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleControls();
        LimitSpeed();
    }

    void HandleControls()
    {
        float targetTorque = 0;
        float targetBreakTorque = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            targetTorque = maxTorque;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            targetTorque = -maxTorque;
        else
            targetBreakTorque = maxBrake;

        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = targetTorque;
            wheel.brakeTorque = targetBreakTorque;
        }


    }

    void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}