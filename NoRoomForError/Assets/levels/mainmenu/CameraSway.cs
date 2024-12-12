using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public float swayAmount = 0.1f;  // The amount of sway
    public float swaySpeed = 1.0f;   // Speed of the sway

    public bool on = true;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition; // Store the original position
    }

    void Update()
    {
        if (on)
        {
            // Create a gentle sway using a sine wave
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            float swayY = Mathf.Sin(Time.time * swaySpeed * 0.5f) * swayAmount;  // Add variation in the Y axis

            // Apply the sway to the camera's position
            transform.localPosition = initialPosition + new Vector3(swayX, swayY, 0f);
        }
    }
}
