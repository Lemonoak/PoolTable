using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlane : MonoBehaviour
{
    Vector3 Normal = Vector3.up;
    Vector3 Right = Vector3.right;

    Vector3 Position = Vector3.zero;
    Vector2 Extension = new Vector2(1.0f, 2.0f);

    Vector3 Cross { get => Vector3.Cross(Right, Normal); }

    const float Thiccness = 0.05f;

    [SerializeField] Material Mat = null;

    Vector3[] MinMax
    {
        get
        {
            Vector3[] corners = Corners;

            return new Vector3[]
            {
                corners[0] + Normal * Thiccness,
                corners[2] - Normal * Thiccness
            };
        }
    }

    public Vector3[] Corners
    {
        get
        {
            Vector3[] cornersReturn = new Vector3[4];

            cornersReturn[0] = Position - Right * Extension.x - Cross * Extension.y;
            cornersReturn[1] = Position + Right * Extension.x - Cross * Extension.y;

            cornersReturn[2] = Position + Right * Extension.x + Cross * Extension.y;
            cornersReturn[3] = Position - Right * Extension.x + Cross * Extension.y;

            return cornersReturn;
        }
    }

    Domain domain = null;
    public Collider Collider { get => domain.Collider; }

    public MyPlane(Vector3 normal, Vector3 right, Vector3 position, Vector2 extension, Material mat, string tag)
    {
        this.Normal = normal;
        this.Right = right;
        this.Position = position;
        this.Extension = extension;
        this.Mat = mat;

        Vector3[] minMax = MinMax;
        domain = new Domain(minMax[0], minMax[1], "Plane", Color.gray, mat, tag);
    }

    public void Reflect(Rigidbody rigidbody)
    {
        rigidbody.velocity = MyPlane.Reflect(rigidbody.velocity, Normal);
    }

    public static Vector3 Reflect(Vector3 velocity, Vector3 normal)
    {
        float dot = Vector3.Dot(velocity, normal);

        if (dot > 0.0f)
            return velocity;

        Vector3 d = Mathf.Abs(dot) * normal * 2.0f;

        return velocity + d;
    }

}
