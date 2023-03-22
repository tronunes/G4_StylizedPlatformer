using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
	public GameObject spike;
	public float spikeHeight = 0.5f;
	public float trapTriggerTime = 2f;
	public float spikesDuration = 1f;
	public float spikesSpeed = 10f;

	private bool allowMoving = false;
	private bool allowActivation = false;
	
	private Vector3 spikeStartPosition;
	private Vector3 spikeEndPosition;

	private Vector3 spikeTargetPosition;

	private bool isActivated = false;	// if player has stepped on trap trigger
	private bool isTriggered = false;	// if spikes are deployed already
	private float trapTimer;


	private void Start()
	{
		trapTimer = trapTriggerTime;

		spikeStartPosition = spike.transform.position;
		spikeEndPosition = spikeStartPosition + new Vector3(0f, spikeHeight, 0f);
	}

	private void Update()
	{
		if(spike.transform.position == spikeStartPosition)
			allowActivation = true;

		if(allowActivation)
		{
			if(isActivated && trapTimer > 0)
				trapTimer -= Time.deltaTime;
			else if(isActivated && trapTimer <= 0)
			{
				if(!isTriggered)
				{
					print("deploy spikes");
					allowActivation = false;
					isTriggered = true;
					spike.transform.position = spikeEndPosition;
					StartCoroutine(DeactivateTrap());
				}
			}
		}		

		if(allowMoving)
			MoveSpike();
	}

	private void MoveSpike()
	{
		spike.transform.position = Vector3.MoveTowards(spike.transform.position, spikeStartPosition, spikesSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(!isActivated)
			{
				print("stepping on trap");
				isActivated = true;
				trapTimer = trapTriggerTime;				
			}			
		}			
	}

	private IEnumerator DeactivateTrap()
	{
		print("deactivation started");
		allowMoving = false;
		yield return new WaitForSeconds(spikesDuration);

		allowMoving = true;
		isActivated = false;
		isTriggered = false;
		//spike.transform.position = spikeStartPosition;
		print("spikes off...");
	}


}
