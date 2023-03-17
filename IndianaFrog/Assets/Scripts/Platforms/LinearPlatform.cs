using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex; // Index of the waypoint the platform is currently travelling towards
    [SerializeField] private float travelTime = 2f; // Between two waypoints
    private float currentRemainingTime; // Time left before reaching the next waypoint 
    private float previousRemainingDistance = Mathf.Infinity; // The remaining distance between the platform and the next waypoint **last frame**

    void Start()
    {
        // Waypoint 0 always is at the start position -> start moving towards the next one
        currentWaypointIndex = 1;

        // Don't allow zero travel time as it would cause ZeroDivisionError
        if (travelTime == 0f) { travelTime = 0.1f; }

        // Reset current time left
        currentRemainingTime = travelTime;
    }

    void FixedUpdate()
    {
        // Set the platform position using linear interpolation
        Vector3 start = waypoints[currentWaypointIndex].localPosition;
        Vector3 end = waypoints[GetNextWaypointIndex()].localPosition;
        float time = 1f - currentRemainingTime / travelTime;
        platform.transform.localPosition = Vector3.Lerp(start, end, time);

        float currentRemainingDistance = Vector3.Distance(platform.transform.localPosition, waypoints[GetNextWaypointIndex()].localPosition);

        // Case: Close enough of the current next waypoint -> Start moving towards the next waypoint in the list
        if (currentRemainingDistance <= 0.05f || currentRemainingDistance > previousRemainingDistance)
        {
            currentWaypointIndex = GetNextWaypointIndex();
            previousRemainingDistance = Mathf.Infinity;
        }
        // Case: moving towards the current next waypoint
        else
        {
            previousRemainingDistance = currentRemainingDistance;
        }

        // Reduce timer or reset to original amount when reaching zero
        currentRemainingTime -= Time.fixedDeltaTime;
        if (currentRemainingTime < 0f)
        {
            currentRemainingTime = travelTime;
        }
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
