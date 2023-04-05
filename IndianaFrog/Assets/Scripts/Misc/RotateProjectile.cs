using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateProjectile : MonoBehaviour
{
	private Quaternion r;
	private Rigidbody projectileRigidbody;


	private void Start()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		// simply align the projectile's rotation with trajectory
		r = Quaternion.LookRotation(projectileRigidbody.velocity);
		transform.rotation = r;
	}
}
