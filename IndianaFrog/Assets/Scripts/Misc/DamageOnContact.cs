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
			other.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);
	}
}
