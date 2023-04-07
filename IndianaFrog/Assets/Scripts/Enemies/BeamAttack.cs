using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
	public Transform firePoint;
	public LineRenderer lineRenderer;
	public ParticleSystem glowEffect;
	public ParticleSystem impactEffect;

	public LayerMask hitLayer;
	public int damageAmount = 1;
	
	private EnemyTotem enemyTotem;


	private void Start()
	{
		enemyTotem = GetComponentInParent<EnemyTotem>();
	}

	private void Update()
	{
		// if the parent is rotating, only then fire the beam
		if(enemyTotem.isRotating)
		{
			ActivateBeam();
			if(!glowEffect.isPlaying)
				glowEffect.Play();
		}			
		else
		{
			lineRenderer.enabled = false;
			if(impactEffect.isPlaying)	
				impactEffect.Stop();

			if(glowEffect.isPlaying)
			{
				glowEffect.Stop();
				glowEffect.Clear();	// have to clear particles also after stopping, or they linger for their lifetime-duration
			}
				
		}
	}

	private void ActivateBeam()
	{
		lineRenderer.enabled = true;

		// if the Raycast hits something, set the end of beam to that point so it will be blocked by obstacles
		if(Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, enemyTotem.beamLength, hitLayer))
		{
			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, hit.point);

			// have to check the state of particle effect first, if it is already playing
			// and we are trying to Play() it, script seems to break (same for stopping it)
			if(!impactEffect.isPlaying)
				impactEffect.Play();
			
			impactEffect.transform.position = hit.point;
			impactEffect.transform.rotation = Quaternion.LookRotation(firePoint.position - hit.point);

			if(hit.transform.CompareTag("Player"))
			{
				if(hit.transform.GetComponent<PlayerHealth>().canTakeDamage)
				{
					// cache the wanted direction in a variable
					Vector3 knockbackDirection = hit.transform.position - hit.point;

					// set the upwards direction manually
					knockbackDirection.y = 0.5f;
					hit.transform.GetComponent<Rigidbody>().AddForce(knockbackDirection * 8f, ForceMode.Impulse);					
				}
				// this sets the boolean so call AFTER checking for knockback
				hit.transform.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);

			}
		}
		else
		{
			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * enemyTotem.beamLength);

			if(impactEffect.isPlaying)
				impactEffect.Stop();
		}			
	}
}
