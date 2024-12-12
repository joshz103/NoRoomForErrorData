using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftrightmovement : MonoBehaviour
{
    public float distance = 2.0f;
    public float speed = 2.0f;
    private Vector3 startPosition;

    public Vector3 spawnOffset;

    void Start()
    {
        transform.position += spawnOffset;
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new X position using a sine wave for smooth up and down movement
        float newX = startPosition.x + Mathf.Sin(Time.time * speed) * distance;

        // Set the objects new position
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
}
