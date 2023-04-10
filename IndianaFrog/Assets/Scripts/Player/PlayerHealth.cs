using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[Header("HEALTH")]
	public int maxHealth = 4;
	public int currentHealth;

	[Header("INVULNERABILITY")]
	public float invulnerabilityTime = 1f;

	[Header("UI")]
	public HealthBar healthbar;

	//Feathers that represent hitpoints
	private GameObject hpFeather0;
	private GameObject hpFeather1;
	private GameObject hpFeather2;

	[Header("FEATHER DAMAGE")]
	public int blinksFeather = 3;
	public float blinkSpeed = 0.1f;


	private bool canTakeDamage = true;


	private void Start()
	{
		//Find the Feathers from headfeather prefab, scene should only have one
		hpFeather0 = GameObject.Find("HeadFeathersHP/hp0");
		hpFeather1 = GameObject.Find("HeadFeathersHP/hp1");
		hpFeather2 = GameObject.Find("HeadFeathersHP/hp2");

		ResetHealth();
	}

	public void AddHealth(int healthToAdd)
	{
		// probably not needed but should prevent integer overflow accidents if they would happen
		healthToAdd = Mathf.Min(healthToAdd, maxHealth - currentHealth);
		currentHealth += healthToAdd;
		healthbar.SetHealth(currentHealth);
	}

	public void SubtractHealth(int healthToSubtract)
	{
		if(canTakeDamage)
		{
			healthToSubtract = Mathf.Min(healthToSubtract, currentHealth);
			currentHealth -= healthToSubtract;
			healthbar.SetHealth(currentHealth);

			if (currentHealth <= 0)
			{
				KillPlayer();
			}
			else
			{
				StartCoroutine(BecomeInvulnerable());
				//print(healthToSubtract + " damage taken.");
				StartCoroutine(FeatherDamage());
			}
		}
	}

	private IEnumerator BecomeInvulnerable()
	{
		canTakeDamage = false;
		yield return new WaitForSeconds(invulnerabilityTime);

		canTakeDamage = true;
	}

	private IEnumerator FeatherDamage()
	{
		GameObject currentFeather;
		switch (currentHealth)
		{
			case 3:
				currentFeather = hpFeather2;
				break;
			case 2:
				currentFeather = hpFeather1;
				break;
			case 1:
				currentFeather = hpFeather0;
				break;
			default:
				Debug.Log("Something went wrong with getting current health");
				currentFeather = null;
				break;
		}
		for (int i = 0; i < blinksFeather; i++)
        {
            // Hide the object
            currentFeather.SetActive(false);
            yield return new WaitForSeconds(blinkSpeed);

            // Show the object
            currentFeather.SetActive(true);
            yield return new WaitForSeconds(blinkSpeed);
        }

        // Hide the object for good
        currentFeather.SetActive(false);
	}

	private void KillPlayer()
	{
		// Prevent spamming death
		canTakeDamage = false;

		// Reset the player to the most recent checkpoint
		gameObject.GetComponent<PlayerCheckpointHandler>().Die();
	}

	public void ResetHealth()
	{
		StartCoroutine(BecomeInvulnerable());	// We can run the Coroutine when the character spawns
												// (might need some testing if useful or abusable)

		currentHealth = maxHealth;
		healthbar.SetMaxHealth(maxHealth);

		// Show all feathers
		hpFeather0.SetActive(true);
		hpFeather1.SetActive(true);
		hpFeather2.SetActive(true);
	}
}
