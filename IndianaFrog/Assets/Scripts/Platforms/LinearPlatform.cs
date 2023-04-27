using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform; // The moving part
    [SerializeField] private Collider meshCollider; // The platform's collider
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex; // Index of the waypoint the platform is currently travelling towards
    [SerializeField] private float travelTime = 3f; // Between two waypoints
    [SerializeField] private float startDelay = 0f;
    private float timeLeft; // Time left before reaching the next waypoint 
    [SerializeField] private AnimationCurve travelTimeCurve; // A curve to smooth out the movement

    void Start()
    {
        // Waypoint 0 always is at the start position -> start moving towards the next one
        currentWaypointIndex = 1;

        // Don't allow zero travel time as it would cause ZeroDivisionError
        if (travelTime == 0f) { travelTime = 0.1f; }

        // Reset current time left
        timeLeft = travelTime;
    }

    void Update()
    {
        // Move only after the start delay
        if (startDelay < 0f)
        {
            // Set the platform position using linear interpolation
            Vector3 startPosition = waypoints[currentWaypointIndex].localPosition;
            Vector3 endPosition = waypoints[GetNextWaypointIndex()].localPosition;
            float lerpTime = travelTimeCurve.Evaluate(1f - timeLeft / travelTime);
            platform.transform.localPosition = Vector3.Lerp(startPosition, endPosition, lerpTime);

            // Case: Reached the current waypoint -> Start moving towards the next waypoint in the list
            if (timeLeft <= 0f)
            {
                currentWaypointIndex = GetNextWaypointIndex();
                timeLeft = travelTime;
            }

            timeLeft -= Time.deltaTime;
        }
        else
        {
            // Reduce delay timer
            startDelay -= Time.deltaTime;
        }

        // In case player is parented to the platform -> update its position as well
        Physics.SyncTransforms();
    }

    int GetNextWaypointIndex()
    {
        int nextIndex;

        if (currentWaypointIndex == waypoints.Count - 1)
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentWaypointIndex + 1;
        }

        return nextIndex;
    }

    int GetPreviousWaypointIndex()
    {
        int previousIndex;

        if (currentWaypointIndex == 0)
        {
            previousIndex = waypoints.Count - 1;
        }
        else
        {
            previousIndex = currentWaypointIndex - 1;
        }

        return previousIndex;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0f, 1f, 0.6f); // Transparent blue
        int index = 0;

        // Draw all the waypoints
        // The first one has a different draw shape to avoid z-fighting
        foreach(Transform waypoint in waypoints)
        {
            if (index != 0)
            {
                Gizmos.DrawCube(
                    waypoint.position,
                    meshCollider.bounds.size
                );
            }
            else
            {
                Gizmos.DrawSphere(
                    waypoint.position,
                    1f
                );
            }
            index++;
        }
    }
}
