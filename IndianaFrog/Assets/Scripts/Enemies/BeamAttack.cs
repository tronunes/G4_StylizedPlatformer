using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
	public Transform firePoint;
	public LayerMask hitLayer;

	public int damageAmount = 1;
	public float damageInterval = 1f;

	private LineRenderer lineRenderer;
	private EnemyTotem enemyTotem;

	private float damageTimer;

	


	private void Start()
	{
		lineRenderer = GetComponentInChildren<LineRenderer>();
		enemyTotem = GetComponentInParent<EnemyTotem>();

		damageTimer = damageInterval = 1f;
	}

	private void Update()
	{
		damageTimer -= Time.deltaTime;

		// if the parent is rotating, only then fire the beam
		if(enemyTotem.isRotating)
			ActivateBeam();
		else
			lineRenderer.enabled = false;
	}

	private void ActivateBeam()
	{
		lineRenderer.enabled = true;

		RaycastHit hit;
		Debug.DrawRay(firePoint.position, firePoint.forward * enemyTotem.beamLength, Color.red);

		// if the Raycast hits something, set the end of beam to that point so it will be blocked by obstacles
		if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, enemyTotem.beamLength, hitLayer))
		{
			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, hit.point);

			// can also implement particle effect instantiate here
			//

			// damage the player only once per "interval", otherwise player takes damage
			// every frame while the beam is passing the player
			if(hit.transform.CompareTag("Player"))
			{
				if(damageTimer <= 0)
					hit.transform.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);

				damageTimer = damageInterval;
			}			
		}
		else
		{
			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * enemyTotem.beamLength);
		}
	}
}
