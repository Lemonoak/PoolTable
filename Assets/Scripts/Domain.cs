using System.Collections.Generic;
using UnityEngine;

public class Domain : MonoBehaviour
{
    Vector3 Min = Vector3.zero;
    Vector3 Max = Vector3.zero;

    GameObject MyGameObject;
    Material material;

    public string Name { get => MyGameObject.name; set => MyGameObject.name = value; }

    public Material Material { get => material; set => material = value; }

    public Vector3 RandomPosition 
    {
        get
        {
            float gap = 0.05f;
            return new Vector3(
                UnityEngine.Random.Range(Min.x + gap, Max.x - gap),
                UnityEngine.Random.Range(Min.y + gap, Max.y - gap),
                UnityEngine.Random.Range(Min.z + gap, Max.z - gap));
        }
    }

    Vector3 position = Vector3.zero;
    public Vector3 Position 
    {
        get => MyGameObject.transform.position;
        set
        {
            MyGameObject.transform.position = value;
        }
    }

    bool visible = true;
    public bool Visible 
    { 
        get => visible;
        set
        {
            visible = value;
            MyGameObject.SetActive(Visible);
        }
    }

    public Collider Collider { get => MyGameObject.GetComponent<Collider>(); }

    public Domain(Vector3 min, Vector3 max, string name, Color color, Material material, string tag)
    {
        this.Min = min;
        this.Max = max;

        Init(name, color, material, tag);
    }

    public Domain(Vector3 position, Vector3 normal, Vector3 right, string name, Color color, Material material, string tag)
    {
        Vector3 forward = Vector3.Cross(right, normal).normalized;

        Min = position - 0.5f * (right + forward + 0.1f * normal);
        Max = position + 0.5f * (right + forward + 0.1f * normal);

        Init(name, color, material, tag);
    }

    private void Init(string name, Color color, Material material, string tag)
    {
        MyGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //UnityEngine.Object.Destroy(MyGameObject.GetComponent<Collider>());
        MyGameObject.GetComponent<Collider>().isTrigger = true;
        MyGameObject.transform.localScale = Max - Min;
        Position = 0.5f * (Min + Max);
        Name = name;

        MyGameObject.GetComponent<Renderer>().material = new Material(material);
        Material = MyGameObject.GetComponent<Renderer>().material;
        Material.color = color;

        MyGameObject.tag = tag;
    }

    public void Debug(float val, float min, float max)
    {
        Material.color = ValueToColor(val, min, max);
    }

    public static Color ValueToColor(float value, float min, float max)
    {
        return Color.HSVToRGB(
            Mathf.Lerp(250.0f / 360.0f, 0.0f, Mathf.InverseLerp(min, max, Mathf.Clamp(value, min, max))),
            1.0f, 1.0f);
    }
}
