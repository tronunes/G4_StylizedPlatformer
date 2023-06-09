using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public Animator animator;
	public ParticleSystem healthParticles;
	public ParticleSystem damageParticles;

	[Header("HEALTH")]
	public int maxHealth = 4;
	public int currentHealth;

	[Header("INVULNERABILITY")]
	public float invulnerabilityTime = 1f;

	[Header("UI")]
	public HealthBar healthbar;
	public FeatherManager featherManager;

	public AudioSource frogDamage;
	public AudioSource frogDeath;

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

		healthParticles.Play();
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
				animator.SetBool("IsAlive", false);
				PlayDeathAnimation();
				frogDeath.Play();
			}
			else
			{
				// Play animation
				animator.SetTrigger("TakeDamage");
				frogDamage.Play();

				StartCoroutine(BecomeInvulnerable());
			}

			damageParticles.Play();
		}
	}

	private IEnumerator BecomeInvulnerable()
	{
		canTakeDamage = false;
		yield return new WaitForSeconds(invulnerabilityTime);

		canTakeDamage = true;
	}

	private void PlayDeathAnimation()
	{
		// Prevent spamming death
		canTakeDamage = false;

		// Disable input
		gameObject.GetComponent<PlayerCheckpointHandler>().EnableOrDisablePlayerInput(false);

		// Play death animation (which will trigger the actual death)
		animator.SetTrigger("Death");
	}

	public void ResetHealth()
	{
		StartCoroutine(BecomeInvulnerable());	// We can run the Coroutine when the character spawns
												// (might need some testing if useful or abusable)

		currentHealth = maxHealth;
		healthbar.SetMaxHealth(maxHealth);
		featherManager.FeatherReset();
		animator.SetBool("IsAlive", true);
	}

	public bool CanTakeDamage()
	{
		return canTakeDamage;
	}
}
