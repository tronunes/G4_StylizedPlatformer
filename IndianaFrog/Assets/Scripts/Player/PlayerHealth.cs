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
	public FeatherManager featherManager;

	private bool canTakeDamage = true;


	private void Start()
	{
		ResetHealth();
	}

	public void AddHealth(int healthToAdd)
	{
		// probably not needed but should prevent integer overflow accidents if they would happen
		healthToAdd = Mathf.Min(healthToAdd, maxHealth - currentHealth);
		featherManager.AddFeather(currentHealth, "AddHealth");
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
			featherManager.RemoveFeather(currentHealth, "SubtractHealth");

			if (currentHealth <= 0)
			{
				KillPlayer();
			}
			else
			{
				StartCoroutine(BecomeInvulnerable());
			}
		}
	}

	private IEnumerator BecomeInvulnerable()
	{
		canTakeDamage = false;
		yield return new WaitForSeconds(invulnerabilityTime);

		canTakeDamage = true;
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
		featherManager.FeatherReset();
	}
}
