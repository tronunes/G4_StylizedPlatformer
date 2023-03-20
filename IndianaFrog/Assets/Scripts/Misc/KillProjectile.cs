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
}
