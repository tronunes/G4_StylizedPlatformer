using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    private Quaternion meshOrignalRotation;
    [SerializeField] private Transform rotationPivot;
    [SerializeField] bool onlyOrbitWithoutRotation = false; // If true, the platform mesh won't rotate when orbiting the pivot
    [SerializeField] bool counterClockwise = false; // Otherwise orbiting is clockwise viewed from above
    [SerializeField] private float orbitingTime;

    void Start()
    {
        meshOrignalRotation = platform.rotation;
    }

    void FixedUpdate()
    {
        float direction = counterClockwise ? -1f : 1f;

        // Rotate the pivot
        rotationPivot.Rotate(0f, direction * orbitingTime, 0f);

        // Move the Platform mesh in the correct position
        if (onlyOrbitWithoutRotation)
        {
            platform.rotation = meshOrignalRotation;
        }
    }
}
