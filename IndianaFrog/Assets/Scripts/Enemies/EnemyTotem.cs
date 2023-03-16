using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTotem : MonoBehaviour
{
	[Header("LINKED OBJECTS")]
	public Transform firePoint;
	public GameObject projectile;	
	
	[Header("TOTEM STATS")]
	public float rotateSpeed = 10;
	public float rotateInterval = 2f;

	[Header("TEMP VARIABLES")]
	public float fireInterval = 3f;
	public float projectileSpeed = 1f;

	private float t;
	private bool allowRotation = true;
	private bool isRotating = true;
	private Quaternion targetRotation;


	private void Start()
	{
		t = fireInterval;
		targetRotation = this.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
	}

	private void Update()
	{
		RotateObject();
		if(Quaternion.Angle(transform.rotation, targetRotation) < 0.001f && allowRotation)
			StartCoroutine(Hold());			

		t -= Time.deltaTime;
		if(t <= 0 && isRotating)
			Fire();
	}

	private void RotateObject()
	{		
		transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
	}

	public IEnumerator Hold()
	{
		//print("HOLD!");
		allowRotation = false;
		isRotating = false;		
		
		yield return new WaitForSeconds(rotateInterval);
		targetRotation = this.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
		allowRotation = true;
		isRotating = true;		
	}

	private void Fire()
	{
		GameObject projectileInstance = Instantiate(projectile, firePoint.position, firePoint.rotation);
		projectileInstance.GetComponent<Rigidbody>().velocity = projectileSpeed * firePoint.forward;

		t = fireInterval;
	}
}
