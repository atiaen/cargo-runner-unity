using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectScript : MonoBehaviour
{
    public float amplitude = 1.0f;  // The maximum displacement from the original position
    public float frequency = 1.0f;  // The speed of the oscillation

    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the object
        originalPosition = transform.position;
    }

    void Update()
    {
        // Calculate new position using sine waves for smooth floating motion
        float newY = originalPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
        float newX = originalPosition.x + amplitude * Mathf.Cos(Time.time * frequency * 0.5f);
        float newZ = originalPosition.z + amplitude * Mathf.Sin(Time.time * frequency * 0.3f);

        // Update the object's position
        transform.position = new Vector3(newX, newY, newZ);
    }
}
