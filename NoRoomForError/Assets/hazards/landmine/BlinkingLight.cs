using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Light light;
    public float time;
    public float intensity;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startLight());
    }

    IEnumerator startLight()
    {
        light.intensity = intensity;

        yield return new WaitForSeconds(time);
        StartCoroutine(stopLight());
    }

    IEnumerator stopLight()
    {
        light.intensity = 0;
        
        yield return new WaitForSeconds(time);
        StartCoroutine(startLight());
    }

}
