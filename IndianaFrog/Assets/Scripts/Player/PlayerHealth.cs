using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[Header("HEALTH")]
	public int maxHealth = 4;
	public int currentHealth;

	//[Header("UI")]
	//public HealthBar healthbar;	// place holders for UI stuff later?


	private void Start()
	{
		currentHealth = maxHealth;
		//healthbar.SetMaxHealth(maxHealth);
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
		//healthbar.SetHealth(currentHealth);
	}

	public void SubtractHealth(int healthToSubtract)
	{
		healthToSubtract = Mathf.Min(healthToSubtract, currentHealth);
		currentHealth -= healthToSubtract;
		//healthbar.SetHealth(currentHealth);

		print(healthToSubtract + " damage taken.");
	}

	private void KillPlayer()
	{
		print("game over man, game over!");
		//Destroy(this.gameObject);
	}
}
