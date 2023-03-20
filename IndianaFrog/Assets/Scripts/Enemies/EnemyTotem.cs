using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTotem : MonoBehaviour
{	
	[Header("TOTEM ATTRIBUTES")]
	public float rotateSpeed = 10;
	public float rotateWaitTime = 2f;
	public float beamLength = 5f;

	private bool allowRotation = true;
	[HideInInspector] public bool isRotating = true;
	private Quaternion targetRotation;


	private void Start()
	{
		targetRotation = this.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
	}

	private void Update()
	{
		RotateObject();
		if(Quaternion.Angle(transform.rotation, targetRotation) < 0.001f && allowRotation)
			StartCoroutine(Hold());
	}	

	private void RotateObject()
	{		
		transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
	}

	public IEnumerator Hold()
	{
		allowRotation = false;
		isRotating = false;		
		
		yield return new WaitForSeconds(rotateWaitTime);
		targetRotation = this.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
		allowRotation = true;
		isRotating = true;		
	}

}
