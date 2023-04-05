using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosion : MonoBehaviour
{
    public GameObject explosionObj;


	private void OnTriggerEnter(Collider other)
	{
		Instantiate(explosionObj, this.transform.position, this.transform.rotation);
	}
}
