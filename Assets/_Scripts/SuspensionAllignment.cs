using System.Collections;
using UnityEngine;

// ADD THIS SCRIPT TO EACH OF THE WHEEL MESHES / WHEEL MESH CONTAINER OBJECTS
public class SuspensionAllignment : MonoBehaviour
{
    public WheelCollider wheelC;

    private Vector3 wheelCCenter;
    private RaycastHit hit;

    // Display
    void Update()
    {
        wheelCCenter = wheelC.transform.TransformPoint(wheelC.center);

        if (Physics.Raycast(wheelCCenter, -wheelC.transform.up, out hit, wheelC.suspensionDistance + wheelC.radius))
        {
            transform.position = hit.point + (wheelC.transform.up * wheelC.radius);
        }
        else
        {
            transform.position = wheelCCenter - (wheelC.transform.up * wheelC.suspensionDistance);
        }

        /* Custom, wheel rotations
        Vector3 euler = transform.rotation.eulerAngles;
        euler.x += Time.deltaTime * wheelC.rpm / 6;
        transform.rotation = Quaternion.Euler(euler);
        */
    }
}

// Source
// https://gist.github.com/victorbstan/4dde0d0b4203c248423e