using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("LINKED OBJECTS")]
	public Transform firePoint;
	public GameObject projectile;

	[Header("MASK TYPE")]
	public bool isStaticEnemy;

	[Header("ENEMY STATS")]
	public float fireInterval = 3f;
	public float projectileSpeed = 1f;
	public float detectionRange = 5f;
	public float rotateSpeed = 10;

	private float fireDelayTimer;
	private Transform target;
	private SphereCollider sc;
	private Vector3 targetDirection;
	private Quaternion lookRotation;
	private Quaternion defaultRotation;


	private void Start()
	{
		// lets set detection radius with Collider and OnTriggerEnter instead of OverlapSphere for more perfomant code
		sc = GetComponent<SphereCollider>();
		sc.radius = detectionRange;

		fireDelayTimer = fireInterval;
		defaultRotation = transform.rotation;
	}

	private void Update()
	{
		if(!isStaticEnemy)
			Rotate();

		fireDelayTimer -= Time.deltaTime;
		if(fireDelayTimer <= 0)
			Fire();
	}

	private void Fire()
	{
		// dont perform checks if mask is set to static, so it doesnt track
		// player and keeps firing at regular intervals
		if(!isStaticEnemy)
		{
			if(target == null)
				return;

			// calculate the amount to turn, if not facing player accurately enough we return
			// from function and do not fire
			float amountToRotate = Mathf.Abs(this.transform.rotation.y) - Mathf.Abs(lookRotation.y);
			if(Mathf.Abs(amountToRotate) > 0.025f)
				return;
		}		

		GameObject projectileInstance = Instantiate(projectile, firePoint.position, firePoint.rotation);
		projectileInstance.GetComponent<Rigidbody>().velocity = projectileSpeed * firePoint.forward;

		fireDelayTimer = fireInterval;
	}

	private void Rotate()
	{
		if(target != null)
		{
			targetDirection = target.position - this.transform.position;
			targetDirection.y = 0f;	// set y.component to zero, so we rotate only around y-axis
			lookRotation = Quaternion.LookRotation(targetDirection);

			// using Quaternion.RotateTowards feels better than Slerp
			transform.rotation = Quaternion.RotateTowards(this.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
		}
		else
		{
			transform.rotation = Quaternion.RotateTowards(this.transform.rotation, defaultRotation, rotateSpeed * Time.deltaTime);
		}
			
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
			target = other.transform;
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
			target = null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, detectionRange);
	}
}
