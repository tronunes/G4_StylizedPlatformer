using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
	public int damageAmount = 1;
	public bool isProjectile = false;


	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
			other.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);

		// destroy this this object if its projectile and collides with anything
		if(isProjectile)
		{			
			// can instantiate destroy particle here
			//
			Destroy(this.gameObject);
		}
	}
}
