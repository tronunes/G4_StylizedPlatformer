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

	private float damageTimer = 0f;
	private bool canTakeDamage = true;


	private void Start()
	{
		damageTimer = invulnerabilityTime;	// might aswell...?

		currentHealth = maxHealth;
		healthbar.SetMaxHealth(maxHealth);
	}

	private void Update()
	{
		if(damageTimer > 0f)
		{
			canTakeDamage = false;
			damageTimer -= Time.deltaTime;
		}
		else
		{
			canTakeDamage = true;
		}

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

			damageTimer = invulnerabilityTime;
			print(healthToSubtract + " damage taken.");
		}		
	}

	private void KillPlayer()
	{
		print("game over man, game over!");
		//Destroy(this.gameObject);
	}
}
