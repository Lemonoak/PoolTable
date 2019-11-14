using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    GameObject Sphere = null;
    Rigidbody RB;

    [SerializeField] float Drag = 1.0f;
    private void Start()
    {
        Sphere = this.gameObject;
        RB = Sphere.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ReflectSphereOnWall();
        ReflectSphereOnSphere();
        DragOnSphere();
    }

    //THIS IS THE FUNCTION WE CREATED TOGETHER DURING CLASS
    //WHEN A BALL HITS A WALL
    //ALSO THE ELASTIC (ENERGY CONSERVED)
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

    //WHEN A BALL HITS ANOTHER BALL, THIS ONE USES MASK TO FIND TAG
    //ALSO THE IMPULSE (ENERGY ADDED TO BALL)
    void ReflectSphereOnSphere()
    {
        Rigidbody rb = Sphere.GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;

        float halfScale = Sphere.transform.localScale.x * 0.5f;

        LayerMask Mask = LayerMask.GetMask("ReflectiveBall");
        Collider[] Collision = Physics.OverlapSphere(transform.position, gameObject.GetComponent<SphereCollider>().radius, Mask);
        
        for(int i = 0;  i < Collision.Length; i++)
        {
            if(Collision[i] != this.gameObject)
            {
                Vector3 direction = Collision[i].gameObject.transform.position - gameObject.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position, direction, out hit, halfScale, Mask))
                {
                    //Debug.Log(direction, this);
                    //Debug.DrawLine(gameObject.transform.position, hit.transform.position, Color.red);
                    hit.transform.position = hit.point + (-hit.normal) * halfScale;
                    //I used addforce since it was the only version i could get working properly and worked fine for impulsive energy
                    hit.rigidbody.AddForce(direction, ForceMode.Impulse);
                }
            }
        }
    }
   
    //THIS IS THE FAKE FRICTION OF THE FLOOR
    //ALSO THE DISSIPATIVE (PARTIAL ENERGY LOSS)
    void DragOnSphere()
    {
        //Tried to use other calculations but the current solution work best and easiest
        //float dragForceMagnitude = (RB.velocity.magnitude * RB.velocity.magnitude) * Drag;

        RB.velocity = RB.velocity * (1.0f - Time.deltaTime * Drag);
    }

    #region oldstuff
    //THIS WAS A OLD ONE USED FOR TESTING KEPT IN FOR MY OWN SAKE
    //WHEN A BALL HITS ANOTHER BALL
    //ALSO THE IMPULSE (ENERGY ADDED TO BALL)
    //void ReflectSphereOnSphere()
    //{
    //    RaycastHit hit;

    //    Rigidbody rb = Sphere.GetComponent<Rigidbody>();
    //    Vector3 velocity = rb.velocity;

    //    float halfScale = Sphere.transform.localScale.x * 0.5f;

    //    if (Physics.Raycast(Sphere.transform.position, velocity.normalized, out hit, halfScale))
    //    {
    //        if (hit.transform.tag == "ReflectiveBall")
    //        {
    //            hit.transform.position = hit.point + (-hit.normal) * halfScale;

    //            hit.rigidbody.velocity = MyPlane.Reflect(velocity, -hit.normal);

    //            if (RB.velocity.magnitude >= 2.0f)
    //            {
    //                RB.velocity -= RB.velocity;
    //            }
    //            else if (RB.velocity.magnitude < 2.0f && RB.velocity.magnitude > 1.0f)
    //            {
    //                RB.velocity = -RB.velocity;
    //            }
    //            else if (RB.velocity.magnitude <= 1.0f)
    //            {
    //                RB.velocity -= RB.velocity * 0.5f;
    //            }
    //        }
    //    }
    //}
    #endregion oldstuff

}
