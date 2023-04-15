using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
	public int healAmount = 1;


	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<PlayerHealth>().AddHealth(healAmount);
			Destroy(this.gameObject);
		}
	}
}
