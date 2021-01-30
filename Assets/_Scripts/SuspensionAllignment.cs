using System.Collections;
using UnityEngine;

// ADD THIS SCRIPT TO EACH OF THE WHEEL MESHES / WHEEL MESH CONTAINER OBJECTS
public class SuspensionAllignment : MonoBehaviour
{
    public WheelCollider wheelC;

    private Vector3 wheelCCenter;
    private RaycastHit hit;

    private void FixedUpdate()
    {
        Vector3 _pos = transform.position;
        Quaternion _quat = transform.rotation;

        wheelC.GetWorldPose(out _pos, out _quat);

        transform.position = _pos;
        transform.rotation = _quat;
    }

    // Display
    /*void FixedUpdate()
    {
        wheelCCenter = wheelC.transform.TransformPoint(wheelC.center);

        if (Physics.Raycast(wheelCCenter, -wheelC.transform.up, out hit, wheelC.suspensionDistance + wheelC.radius))
        {
            transform.position = hit.point + (wheelC.transform.up * wheelC.radius);
        }
        else
        {
            transform.position = wheelCCenter - (wheelC.transform.up * wheelC.suspensionDistance);
        }*/

    /* Custom, wheel rotations
    Vector3 euler = transform.rotation.eulerAngles;
    euler.x += Time.deltaTime * wheelC.rpm / 6;
    transform.rotation = Quaternion.Euler(euler);
    */
}

// Source
// https://gist.github.com/victorbstan/4dde0d0b4203c248423e

/*
private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
{
    Vector3 _pos = _transform.position;
    Quaternion _quat = _transform.rotation;

    _collider.GetWorldPose(out _pos, out _quat);

    _transform.position = _pos;
    _transform.rotation = _quat;
}
*/