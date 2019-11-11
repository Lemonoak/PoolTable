using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameObject Sphere = null;
    Rigidbody RB;
    bool IsColliding = false;
    private void Start()
    {
        Sphere = this.gameObject;
        RB = Sphere.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ReflectSphereOnWall();
        //ReflectSphereOnSphere();
        ReflectSphereOnSphereTWO();
    }

    void ReflectSphereOnWall()
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

    //WHEN A BALL HITS ANOTHER BALL
    void ReflectSphereOnSphere()
    {
        RaycastHit hit;

        Rigidbody rb = Sphere.GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;

        float halfScale = Sphere.transform.localScale.x * 0.5f;

        if (Physics.Raycast(Sphere.transform.position, velocity.normalized, out hit, halfScale))
        {
            if (hit.transform.tag == "ReflectiveBall")
            {
                hit.transform.position = hit.point + (-hit.normal) * halfScale;

                hit.rigidbody.velocity = MyPlane.Reflect(velocity, -hit.normal);

                if (RB.velocity.magnitude >= 2.0f)
                {
                    RB.velocity -= RB.velocity;
                }
                else if (RB.velocity.magnitude < 2.0f && RB.velocity.magnitude > 1.0f)
                {
                    RB.velocity = -RB.velocity;
                }
                else if (RB.velocity.magnitude <= 1.0f)
                {
                    RB.velocity -= RB.velocity * 0.5f;
                }
            }
        }
    }

    void ReflectSphereOnSphereTWO()
    {
        Rigidbody rb = Sphere.GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;

        float halfScale = Sphere.transform.localScale.x * 0.5f;

        LayerMask Mask = LayerMask.GetMask("ReflectiveBall");

        Collider[] Collision = Physics.OverlapSphere(transform.position, gameObject.GetComponent<SphereCollider>().radius, Mask);
        
        for(int i = 0;  i < Collision.Length; i++)
        {
            if(Collision[i] != this)
            {
                Vector3 direction = Collision[i].gameObject.transform.position - gameObject.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position, direction, halfScale, Mask))
                {
                    Collision[i].gameObject.GetComponent<Rigidbody>().velocity = MyPlane.Reflect(velocity, direction);

                }
            }
        }
    }
   
    

}
