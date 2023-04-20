using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillProjectile : MonoBehaviour
{
	public float projectileLifeTime = 2f;
	public float projectileDestroyDelayTime = 0.1f;


	private void Start()
	{
		StartCoroutine(Destroy());
	}

	public IEnumerator Destroy()
	{
		yield return new WaitForSeconds(projectileLifeTime);
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Do not destroy if hit by groundcheck, deletes fireball without doing damage
		if(other.name != "GroundCheck")
		{
			// needs a small delay after contact to blend in better with explosion vfx
			Destroy(this.gameObject, projectileDestroyDelayTime);
		}
	}
}
