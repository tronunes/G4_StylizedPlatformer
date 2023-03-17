using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex; // Index of the waypoint the platform is currently travelling towards
    [SerializeField] private float travelTime = 3f; // Between two waypoints
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

    void FixedUpdate()
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

        timeLeft -= Time.fixedDeltaTime;
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
}
