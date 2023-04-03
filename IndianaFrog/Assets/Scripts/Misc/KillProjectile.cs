using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillProjectile : MonoBehaviour
{
	public float projectileLifeTime = 2f;


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
		// needs a small delay after contact to blend in better with explosion vfx
		Destroy(this.gameObject, 0.1f);
	}

}
