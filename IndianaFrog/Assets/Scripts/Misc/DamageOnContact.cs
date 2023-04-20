using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damageAmount = 1;

    private Vector3 knockbackDirection; // Unit vector in the direction of the player's center from the knockbackOriginPoint

    [SerializeField] private Transform knockbackOriginPoint;
    [SerializeField] private float knockbackForce; // Magnitude of the knockback

    // using OnTriggerStay instead of OnTriggerEnter because of continuous lava damage,
    // having both modes makes player take damage twice on the first frame of contact
    private void OnTriggerStay(Collider other)
    {
        if (other) // Other-if statements cause errors without this
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<PlayerHealth>().CanTakeDamage())
                {
                    if (knockbackOriginPoint != null)
                    {
                        // Calculate direction of knockback
                        knockbackDirection = (other.transform.position - knockbackOriginPoint.position);
                    }

                    // Standardize knockback height
                    knockbackDirection.y = 0f;
                    knockbackDirection.Normalize();
                    knockbackDirection.y = 0.6f;

                    other.GetComponent<PlayerMovement>().Knockback(knockbackDirection * knockbackForce);
                    other.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);
                }
            }
        }
    }
}
