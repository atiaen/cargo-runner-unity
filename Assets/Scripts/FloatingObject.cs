using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectScript : MonoBehaviour
{
    public float amplitude = 1.0f;  // The maximum displacement from the original position
    public float frequency = 1.0f;  // The speed of the oscillation

    private Vector3 originalPosition;

    public bool is2D = false;

    void Start()
    {
        // Store the original position of the object
        originalPosition = transform.position;
    }

    void Update()
    {

        if (is2D)
        {
            // Calculate floating offset using sine waves for smooth floating motion
            float newY = amplitude * Mathf.Sin(Time.time * frequency);
            float newX = amplitude * Mathf.Cos(Time.time * frequency * 0.5f);  // Adjust frequency multiplier to change the motion pattern

            // Apply floating offset to the original position
            Vector3 floatingOffset = new Vector3(newX, newY, 0);
            transform.position = originalPosition + floatingOffset;
        }
        else
        {
            // Calculate new position using sine waves for smooth floating motion
            float newY = originalPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
            float newX = originalPosition.x + amplitude * Mathf.Cos(Time.time * frequency * 0.5f);
            float newZ = originalPosition.z + amplitude * Mathf.Sin(Time.time * frequency * 0.3f);

            // Update the object's position
            transform.position = new Vector3(newX, newY, newZ);
        }

    }
}
