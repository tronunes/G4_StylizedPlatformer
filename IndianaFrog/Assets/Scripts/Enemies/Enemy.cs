using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("LINKED OBJECTS")]
	public Transform firePoint;
	public GameObject projectile;
	public LayerMask detectionLayer;

	[Header("ENEMY STATS")]
	public float fireInterval = 3f;
	public float projectileSpeed = 1f;
	public float detectionRange = 5f;
	public float rotateSpeed = 5f;

	private float t;
	private Transform target;
	private Vector3 targetDirection;
	private Quaternion lookRotation;
	private Quaternion defaultRotation;


	private void Start()
	{
		t = fireInterval;
		defaultRotation = transform.rotation;

	}

	private void Update()
	{
		DetectPlayer();
		Rotate();
		target = null;	// target pitaa poistaa koska OverlapSpheren jalkeen se jaa pysyvasti muistiin
						// (ehka OnTriggerEnter ja Exit mielummin?)

		t -= Time.deltaTime;
		if(t <= 0)
			Fire();
	}

	private void Fire()
	{
		t = fireInterval;

		GameObject projectileInstance = Instantiate(projectile, firePoint.position, firePoint.rotation);
		projectileInstance.GetComponent<Rigidbody>().velocity = projectileSpeed * firePoint.forward; 
	}

	private void Rotate()
	{
		if(target != null)
		{
			targetDirection = target.position - this.transform.position;
			targetDirection.y = 0f;	// halutaan etta kaannytaan vain y-akselin ympari
			lookRotation = Quaternion.LookRotation(targetDirection);
			
			transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
		}
		else
		{
			transform.rotation = Quaternion.Slerp(this.transform.rotation, defaultRotation, rotateSpeed * Time.deltaTime);
		}
			
	}

	private void DetectPlayer()
	{
		Collider[] hit = Physics.OverlapSphere(this.transform.position, detectionRange, detectionLayer);
		foreach(Collider playerObject in hit)
			target = playerObject.transform;
	}

	private void ResetTimer()
	{
		//t = 1 / rateOfFire;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, detectionRange);
	}
}
