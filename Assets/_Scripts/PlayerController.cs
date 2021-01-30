using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public WheelCollider[] wheels;
    public WheelCollider[] wheelsFront;
    public WheelCollider[] wheelsMiddle;
    public WheelCollider[] wheelsBack;
    public Transform headJoint;

    [Header("Movement")]
    public Vector2 torqueScalar;
    public Vector2 steeringScalar;
    public Vector2 speedFalloff;
    public Vector2 steeringFalloff;
    public float maxBrake = 250;
    public float maxSpeed = 12; // (m/s)
    public float steerDamping = 5;

    [Header("Camera")]
    public Transform lookTarget;
    public Transform followTarget;
    public float lookTargetSpeed;
    public float lookTargetDist;
    public Vector2 followTargetDist;
    public float followTargetRot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleControls();
        LimitSpeed();
    }


    protected float camSwerveFac = 0;
    void HandleControls()
    {
        float torqueDirr = 0, steerDirr = 0, targetBreakTorque = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            torqueDirr = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            torqueDirr = -1;
        else
            targetBreakTorque = maxBrake;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            steerDirr = -1;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            steerDirr = 1;


        foreach (WheelCollider wheel in wheels)
        {
            wheel.brakeTorque = targetBreakTorque;
        }

        foreach (WheelCollider wheel in wheelsFront)
        {
            float targetAngle = steerDirr * GetMaxAngleAtSpeed(rb.velocity.magnitude);
            wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, targetAngle, Time.deltaTime * steerDamping);
        }

        foreach (WheelCollider wheel in wheelsMiddle)
        {
            wheel.motorTorque = torqueDirr * GetTorqueAtSpeed(rb.velocity.magnitude);
        }

        foreach (WheelCollider wheel in wheelsBack)
        {
            wheel.motorTorque = torqueDirr * GetTorqueAtSpeed(rb.velocity.magnitude);
            float targetAngle = -steerDirr * GetMaxAngleAtSpeed(rb.velocity.magnitude);
            wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, targetAngle, Time.deltaTime * steerDamping);
        }

        // Turns camera when turning, for better view
        if (steerDirr != 0)
        {
            float fac = 1;
            if (Mathf.Sign(camSwerveFac) != Mathf.Sign(steerDirr))
                fac = 2f; // Increases speed when returning to neutral AND turning that way
            camSwerveFac = Mathf.Clamp(camSwerveFac + Time.deltaTime * lookTargetSpeed * fac * steerDirr, -1, 1);
        }
        else if (Mathf.Abs(camSwerveFac) > 0.01f)
        {
            steerDirr = -Mathf.Sign(camSwerveFac);
            camSwerveFac = Mathf.Clamp(camSwerveFac + Time.deltaTime * lookTargetSpeed * steerDirr, -1, 1);
        }
        else
            camSwerveFac = 0;

        if (lookTarget)
            lookTarget.localPosition = Vector3.right * camSwerveFac * lookTargetDist;
        if (followTarget)
        {
            followTarget.localPosition = -Vector3.forward * ((Mathf.Abs(camSwerveFac) * (followTargetDist.y-followTargetDist.x)) + followTargetDist.x);
            followTarget.parent.localRotation = Quaternion.Euler(new Vector3(0, camSwerveFac * followTargetRot, 0));
        }
        if (headJoint)
            headJoint.localRotation = Quaternion.Euler(new Vector3(0, camSwerveFac * 35, 0));
    }

    float GetTorqueAtSpeed (float _speed)
    {
        if (_speed < speedFalloff.x)
            return torqueScalar.y;
        else if (_speed > speedFalloff.y)
            return torqueScalar.x;
        else
        {
            float fac = (_speed - speedFalloff.x) / (speedFalloff.y - speedFalloff.x);
            float torq = ((1-fac) * (torqueScalar.y - torqueScalar.x)) + torqueScalar.x;
            return torq;
        }
    }

    float GetMaxAngleAtSpeed (float _speed)
    {
        if (_speed < steeringFalloff.x)
            return steeringScalar.x;
        else if (_speed > steeringFalloff.y)
            return steeringScalar.y;
        else
        {
            float fac = (_speed - steeringFalloff.x) / (steeringFalloff.y - steeringFalloff.x);
            float angle = fac * (steeringScalar.y - steeringScalar.x) + steeringScalar.x;
            return angle;
        }
    }    

    void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    [NaughtyAttributes.Button]
    void ConfigureWheelParamaters()
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.ConfigureVehicleSubsteps(5,12,14);
        }
    }
}





/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour {

	public void GetInput()
	{
		m_horizontalInput = Input.GetAxis("Horizontal");
		m_verticalInput = Input.GetAxis("Vertical");
	}

	private void Steer()
	{
		m_steeringAngle = maxSteerAngle * m_horizontalInput;
		frontDriverW.steerAngle = m_steeringAngle;
		frontPassengerW.steerAngle = m_steeringAngle;
	}

	private void Accelerate()
	{
		frontDriverW.motorTorque = m_verticalInput * motorForce;
		frontPassengerW.motorTorque = m_verticalInput * motorForce;
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontDriverW, frontDriverT);
		UpdateWheelPose(frontPassengerW, frontPassengerT);
		UpdateWheelPose(rearDriverW, rearDriverT);
		UpdateWheelPose(rearPassengerW, rearPassengerT);
	}

	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.rotation = _quat;
	}

	private void FixedUpdate()
	{
		GetInput();
		Steer();
		Accelerate();
		UpdateWheelPoses();
	}

	private float m_horizontalInput;
	private float m_verticalInput;
	private float m_steeringAngle;

	public WheelCollider frontDriverW, frontPassengerW;
	public WheelCollider rearDriverW, rearPassengerW;
	public Transform frontDriverT, frontPassengerT;
	public Transform rearDriverT, rearPassengerT;
	public float maxSteerAngle = 30;
	public float motorForce = 50;
}
© 2021 GitHub, Inc.
Terms
Privacy
Security
Status
Docs
Contact GitHub
Pricing
API
Training
Blog
About
*/