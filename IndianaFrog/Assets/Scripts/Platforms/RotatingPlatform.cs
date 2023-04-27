using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform; // The actual moving mesh
    private Quaternion meshOrignalRotation;
    [SerializeField] private Transform rotationPivot;
    private Quaternion pivotOriginalRotation;
    [SerializeField] bool onlyOrbitWithoutRotation = false; // If true, the platform mesh won't rotate when orbiting the pivot
    [SerializeField] bool counterClockwise = false; // If false -> orbiting is clockwise (viewed from above)
    [SerializeField] private float orbitingTime = 8f;
    private float timeLeft; // Time left before next orbit begins

    void Start()
    {
        // Save the original rotations at Start
        meshOrignalRotation = platform.rotation;
        pivotOriginalRotation = rotationPivot.localRotation;

        // Reset current time left
        timeLeft = orbitingTime;
    }

    void Update()
    {
        // Calculate values for linear interpolation to get the target yaw angle
        float startYaw = pivotOriginalRotation.eulerAngles.y;
        float endYaw = pivotOriginalRotation.eulerAngles.y + 360f;
        float lerpTime = counterClockwise ?
            timeLeft / orbitingTime :
            1f - timeLeft / orbitingTime;
        float targetYaw = Mathf.Lerp(startYaw, endYaw, lerpTime);

        // Rotate the pivot
        rotationPivot.localRotation = Quaternion.Euler(
            pivotOriginalRotation.eulerAngles.x,
            targetYaw,
            pivotOriginalRotation.eulerAngles.z
        );

        // Keep Platform's original rotation if needed
        if (onlyOrbitWithoutRotation)
        {
            platform.rotation = meshOrignalRotation;
        }

        // Reduce timer or reset it back to original value
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f) { timeLeft = orbitingTime; }

        // In case player is parented to the platform -> update its position as well
        Physics.SyncTransforms();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.6f); // Transparent teal

        // Draw a sphere at the rotating pivot point
        Gizmos.DrawSphere(
            rotationPivot.position,
            1f
        );
    }
}
