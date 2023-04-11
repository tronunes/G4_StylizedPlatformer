using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
	public int damageAmount = 1;
	public Transform knockbackOffsetPoint;


	// using OnTriggerStay instead of OnTriggerEnter because of continuous lava damage,
	// having both modes makes player take damage twice on the first frame of contact
	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(other.GetComponent<PlayerHealth>().canTakeDamage)
			{
				// cache the wanted direction in a variable
				//Vector3 knockbackDirection = other.transform.position - this.transform.position;
				Vector3 knockbackDirection;

				// calculate vector depending if object is projectile or lava
				if(knockbackOffsetPoint != null)
					knockbackDirection = other.transform.position - knockbackOffsetPoint.position;
				else
					knockbackDirection = other.transform.position - this.gameObject.GetComponent<Collider>().ClosestPoint(other.transform.position);

				// normalize the vector size between 0...1
				knockbackDirection = Vector3.Normalize(knockbackDirection);

				// set the upwards direction manually
				knockbackDirection.y = 0.8f;	// 4.0
				print(knockbackDirection);
				//other.GetComponent<Rigidbody>().AddForce(knockbackDirection * 1.0f, ForceMode.Impulse);
				other.GetComponent<PlayerMovement>().AddExternalVelocity(knockbackDirection * 1.5f);
			}
			// this sets the boolean so call AFTER checking for knockback
			other.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);
			
		}			
	}
}
