using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
	public int damageAmount = 1;


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
				Vector3 knockbackDirection = other.transform.position - this.gameObject.GetComponent<Collider>().ClosestPoint(other.transform.position);

				// set the upwards direction manually
				knockbackDirection.y = 0.5f;
				other.GetComponent<Rigidbody>().AddForce(knockbackDirection * 8.0f, ForceMode.Impulse);
			}
			// this sets the boolean so call AFTER checking for knockback
			other.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);
			
		}			
	}
}
