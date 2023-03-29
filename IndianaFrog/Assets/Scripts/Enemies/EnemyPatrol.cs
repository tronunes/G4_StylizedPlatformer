using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
	[Header("PATROL WAYPOINTS")]
	public Vector3[] localWaypoints;

	[Header("ATTRIBUTES")]
	public bool isCyclic = true;
	public float speed = 1.0f;
	public float waitTime = 0.15f;
	public float rotateSpeed;

	private Vector3[] globalWaypoints;
	private int fromWaypointIndex;
	private int toWaypointIndex;
	private float percentBetweenWaypoints;
	private float nextMoveTime;

	private Vector3 newPosition;
	private Quaternion newRotation;


	private void Start()
	{
		globalWaypoints = new Vector3[localWaypoints.Length];
		for(int i = 0; i < localWaypoints.Length; ++i)
			globalWaypoints[i] = localWaypoints[i] + this.transform.position;
	}

	private void Update()
	{
		CalculateMovement();
		Rotate();
		newPosition.y = this.transform.position.y;		
		transform.position = Vector3.MoveTowards(this.transform.position, newPosition, speed * Time.deltaTime);		
	}

	private void CalculateMovement()
	{		
		if(Time.time < nextMoveTime)
			return;

		fromWaypointIndex %= globalWaypoints.Length;
		toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);

		newPosition = globalWaypoints[toWaypointIndex];
		if(percentBetweenWaypoints >= 1f)
		{
			percentBetweenWaypoints = 0f;
			++fromWaypointIndex;

			if(!isCyclic)
			{
				if(fromWaypointIndex >= globalWaypoints.Length - 1)
				{
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
			}
			nextMoveTime = Time.time + waitTime;			
		}
	}

	private void Rotate()
	{
		Vector3 targetDirection = newPosition - this.transform.position;
		targetDirection.y = 0f; // set y.component to zero, so we rotate only around y-axis
		if(targetDirection != Vector3.zero)
			newRotation = Quaternion.LookRotation(targetDirection);

		//transform.rotation = newRotation;
		transform.rotation = Quaternion.RotateTowards(this.transform.rotation, newRotation, rotateSpeed);
	}

	private void OnDrawGizmos()
	{
		if(localWaypoints != null)
		{
			Gizmos.color = Color.red;
			float size = 0.3f;  // marker length

			for(int i = 0; i < localWaypoints.Length; ++i)
			{
				Vector3 globalWaypointPosition = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + this.transform.position;
				Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);
			}
		}
	}
}
