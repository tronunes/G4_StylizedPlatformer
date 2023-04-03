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


	private bool canTakeDamage = true;


	private void Start()
	{
		StartCoroutine(BecomeInvulnerable());	// We can run the Coroutine when the character spawns
												// (might need some testing if useful or abusable)

		currentHealth = maxHealth;
		healthbar.SetMaxHealth(maxHealth);
	}

	private void Update()
	{
		if(currentHealth <= 0)
			KillPlayer();
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

			StartCoroutine(BecomeInvulnerable());
			//print(healthToSubtract + " damage taken.");
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
		// Do something here when player dies
		//Destroy(this.gameObject);
	}
}
