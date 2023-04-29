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
	private float activationAnimationLength = 0.5f;

	private bool allowRotation = true;
	[HideInInspector] public bool isRotating = true;
	private Quaternion targetRotation;
	public List<Animator> animators = new List<Animator>(); // Has to be a list because of the QuadSpinningTotem


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
		
		// Case: rotate wait time is long enough to have the activation animation
		if (rotateWaitTime >= activationAnimationLength)
		{
			// Warn the Player about the activating laser by triggering an animation before activating the laser
			yield return new WaitForSeconds(rotateWaitTime - activationAnimationLength);
			foreach (Animator animator in animators)
			{
				animator.SetTrigger("Activate");
			}

			yield return new WaitForSeconds(activationAnimationLength);
			SetNewTargetRotation();	
		}
		// Case: no animation because the rotate wait time is too small
		else
		{
			yield return new WaitForSeconds(rotateWaitTime);
			SetNewTargetRotation();	
		}
	}

	public void SetNewTargetRotation()
	{
		targetRotation = this.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
		allowRotation = true;
		isRotating = true;	
	}

}
