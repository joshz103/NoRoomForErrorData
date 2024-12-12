using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dimlight : MonoBehaviour
{
    public Light light;
    public float startIntensity;
    public float fadeDuration = 0.25f;

    // Update is called once per frame
    void Update()
    {
        if (light.intensity > 0)
        {
            // Calculate the decrease based on time
            float intensityDecrease = startIntensity * (Time.deltaTime / fadeDuration);
            light.intensity -= intensityDecrease;

            // Ensure the intensity doesn't go below 0
            if (light.intensity < 0)
            {
                light.intensity = 0;
            }
        }
    }
}
