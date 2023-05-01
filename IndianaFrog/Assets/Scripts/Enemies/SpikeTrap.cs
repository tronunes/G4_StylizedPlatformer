using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
	public GameObject spike;
	public Animator animator;
	public float spikeHeight = 0.5f;
	public float trapTriggerTime = 2f;
	public float spikesDuration = 1f;
	public float spikesSpeed = 10f;
	public float spikesRetractSpeed = 0.8f;	

	public AudioSource trapActivate;
	public AudioSource trapLaunch;
	
	private Vector3 spikeStartPosition;
	private Vector3 spikeEndPosition;

	private bool allowActivation = true;	// set true when the spikes have retracted to their starting position
	private bool isActivated = false;		// if player has stepped on trap trigger
	private bool isTriggered = false;		// if spikes are deployed already
	
	private float trapTimer;


	private void Start()
	{
		trapTimer = trapTriggerTime;

		spikeStartPosition = spike.transform.position;
		spikeEndPosition = spikeStartPosition + new Vector3(0f, spikeHeight, 0f);
	}

	private void Update()
	{
		// resets the trap if spikes are moved to starting position
		if(spike.transform.position == spikeStartPosition)
			allowActivation = true;

		// if the trap is reset, check if player has triggered it
		if(allowActivation)
		{
			if(isActivated && trapTimer > 0)
				trapTimer -= Time.deltaTime;
			else if(isActivated && trapTimer <= 0)
			{
				if(!isTriggered)
				{
					//print("deploy spikes");
					allowActivation = false;
					isTriggered = true;
					StartCoroutine(DeactivateTrap());
				}
			}
		}		

		// then attempt to move spikes
		MoveSpikes();
	}

	private void MoveSpikes()
	{
        if (isTriggered)
        {
            spike.transform.position = Vector3.MoveTowards(spike.transform.position, spikeEndPosition, spikesSpeed * Time.deltaTime);
			trapLaunch.Play();
        }
        else if (!isTriggered && !allowActivation)
        {
            spike.transform.position = Vector3.MoveTowards(spike.transform.position, spikeStartPosition, spikesRetractSpeed * Time.deltaTime);
        }
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(!isActivated)
			{
				//print("stepping on trap");
				isActivated = true;
				trapTimer = trapTriggerTime;
				animator.SetTrigger("Activate");
				trapActivate.Play();
			}
		}
	}

	private IEnumerator DeactivateTrap()
	{
		//print("deactivation started");
		yield return new WaitForSeconds(spikesDuration);

		isActivated = false;
		isTriggered = false;
		//print("spikes off...");
	}


}
