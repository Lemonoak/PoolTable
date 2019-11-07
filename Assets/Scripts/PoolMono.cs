using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMono : MonoBehaviour
{
    MyPlane MyPlane = null;

    [SerializeField] Vector3 Normal = Vector3.up;
    [SerializeField] Vector3 Right = Vector3.right;
    [SerializeField] Vector3 Position = Vector3.zero;
    [SerializeField] Vector2 Extension = new Vector2(1.0f, 2.0f);

    [SerializeField] Material Mat = null;

    [SerializeField] string Tag = "Reflective";

    [SerializeField] GameObject Sphere = null;

    //Only fires once when you put script on object, recompile or change something in the inspector
    private void OnValidate() {}

    void Start()
    {
        //MyPlane = new MyPlane(Normal, Right, Position, Extension, Mat, Tag);

        //Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Sphere.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
        //Sphere.transform.localScale = Vector3.one * 0.2f;
        //Rigidbody RB = Sphere.AddComponent<Rigidbody>();
        //RB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //RB.useGravity = false;
        //RB.velocity = new Vector3(0.0f, -1.0f, -1.0f);

        //PlaneDetector spherePlaneDetector = Sphere.AddComponent<PlaneDetector>();
        //spherePlaneDetector.Plane = MyPlane;

        Sphere.GetComponent<Rigidbody>().velocity = new Vector3(2.0f, 0.0f, 2.0f);
    }

    private void FixedUpdate()
    {
        ReflectSphere();
    }

    void ReflectSphere()
    {
        RaycastHit hit;

        Rigidbody rb = Sphere.GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;

        float halfScale = Sphere.transform.localScale.x * 0.5f;

        if (Physics.Raycast(Sphere.transform.position, velocity.normalized, out hit, halfScale))
        {
            if (hit.transform.tag == "Reflective")
            {
                Sphere.transform.position = hit.point + hit.normal * halfScale;

                rb.velocity = MyPlane.Reflect(velocity, hit.normal);
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        Vector3[] corners = MyPlane.Corners;

        for (int i = 0; i < corners.Length; i++)
        {
            Gizmos.DrawSphere(corners[i], 0.1f);
        }
    }*/
}
