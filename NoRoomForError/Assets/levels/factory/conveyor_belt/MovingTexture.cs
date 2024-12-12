using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    public int materialIndex = 0;
    public Material material1;
    public SkinnedMeshRenderer rend;

    //public float moveXspeed;
    public float moveYspeed;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<SkinnedMeshRenderer>() != null)
        {
            rend = GetComponent<SkinnedMeshRenderer>();
            material1 = rend.materials[materialIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.deltaTime * moveYspeed;  // Calculate the offset over time
        material1.mainTextureOffset = new Vector2(0, material1.mainTextureOffset.y + offset);  // Move the UVs along the Y-axis
        if(material1.mainTextureOffset.y >= 1)
        {
            material1.mainTextureOffset = new Vector2(0, 0);
        }

    }
}
