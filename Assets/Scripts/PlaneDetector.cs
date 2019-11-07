using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDetector : MonoBehaviour
{
    MyPlane plane = null;

    public MyPlane Plane { get => plane; set => plane = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other != Plane.Collider)
            return;
        Plane.Reflect(GetComponent<Rigidbody>());
    }
}
