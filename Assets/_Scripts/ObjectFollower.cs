using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    public Transform target;
    public Transform lookTarget;
    public Vector3 offset;

    public bool useRigidbody;
    private Rigidbody rb;

    public bool useRelativeDirection = false;
    public bool useVerticalRot = true;

    public bool runOnUpdate = true;

    public float positionDamping = 5f;
    public float rotationDamping = 5f;

    
    private float yOffset;

    private void Start()
    {
        if (!runOnUpdate)
            Destroy(this);

        rb = target.GetComponent<Rigidbody>();
        if (useRigidbody && !rb)
            Debug.Log("Can't find rigidbody on " + target.name);

        RunOnce();
    }

    [NaughtyAttributes.Button]
    void RunOnce()
    {
        bool _cache = useVerticalRot;
        useVerticalRot = false;
        LateUpdate();
        useVerticalRot = _cache;
        yOffset = transform.position.y - target.position.y;
    }

    void LateUpdate()
    {
        if (target)
            UpdatePosition();
        if (lookTarget)
            UpdateRotation();
    }

    void UpdatePosition()
    {
        Vector3 _pos = Vector3.zero, _target;

        if (useRigidbody && rb)
            _target = rb.position;
        else
            _target = target.position;

        if (useRelativeDirection)
        {
            _pos = _target + (target.right * offset.x) + (target.up * offset.y) + (target.forward * offset.z);
            if (useVerticalRot)
                _pos.y = _target.y + yOffset;
        }
        else if (useVerticalRot)
            _pos.y = _target.y + yOffset; // ??? IDK, ignore this setting
        else
            _pos = _target + offset;

        transform.position = Vector3.Lerp(transform.position, _pos, positionDamping * Time.deltaTime);
    }

    void UpdateRotation()
    {
        // Calculate the current rotation angles
        var wantedRotationAngle = lookTarget.eulerAngles.y;

        var currentRotationAngle = transform.eulerAngles.y;

        if (Mathf.Abs(currentRotationAngle - wantedRotationAngle) < 2)
            return;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Always look at the target
        transform.LookAt(lookTarget);
    }
}

// Rotation code taking from SmoothFollow.cs, Unity asset