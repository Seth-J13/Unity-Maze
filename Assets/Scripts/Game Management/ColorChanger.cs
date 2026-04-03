using UnityEngine;


public class ColorChanger : MonoBehaviour
{

    public Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material m = mr.material;
        m.color = color;
        mr.material = m;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
