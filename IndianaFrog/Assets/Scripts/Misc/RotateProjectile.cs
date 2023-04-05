using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateProjectile : MonoBehaviour
{
	private Quaternion r;
	private Rigidbody rigidbody;


	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	private void Update()
	{
		// simply align the projectile's rotation with trajectory
		r = Quaternion.LookRotation(rigidbody.velocity);
		transform.rotation = r;
	}
}
