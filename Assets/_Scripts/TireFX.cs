using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireFX : MonoBehaviour
{
    public GameObject dust;
    public Vector2 breakScalar;
    public float breakingDustRate = 100;
    public float drivingDustRate = 10;

    public AudioSource audioSourceGravel;
    public Vector2 roadScalar;
    public float roadMaxVol;



    private WheelCollider wc;

    // Start is called before the first frame update
    void Start()
    {
        wc = GetComponent<WheelCollider>();
        if (!wc)
            Debug.LogWarning("Could not find WheelCollider on " + name);
    }

    void FixedUpdate()
    {
        if (!wc.isGrounded)
        {
            DisableRoadNoises();
            return;
        }

        float _velocity = wc.attachedRigidbody.velocity.magnitude;

        if (_velocity >= breakScalar.x && wc.brakeTorque >= 50)
            Burnout(_velocity);

        if (_velocity >= roadScalar.x)
            RoadNoises(_velocity);
        else
            DisableRoadNoises();
    }

    void Burnout (float _velocity)
    {
        if (dust)
        {
            float _fac = 1;

            if (_velocity < roadScalar.y)
                _fac = (_velocity - breakScalar.x) / (breakScalar.y - breakScalar.x);

            if (Random.value <= Time.deltaTime * breakingDustRate * _fac)
                Instantiate(dust, transform.position, Quaternion.identity);
        }

    }

    void RoadNoises (float _velocity)
    {
        float _fac = 1;

        if (_velocity < roadScalar.y)
            _fac = (_velocity - roadScalar.x) / (roadScalar.y - roadScalar.x);

        if (Random.value <= Time.deltaTime * drivingDustRate * _fac)
            Instantiate(dust, transform.position, Quaternion.identity);

        audioSourceGravel.volume = roadMaxVol * _fac;
    }

    void DisableRoadNoises ()
    {
        audioSourceGravel.volume = 0;
    }
}