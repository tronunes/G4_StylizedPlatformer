using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private int maxHP = 3;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    void RecoverHP()
    {
        currentHP++;

        // Clamp HP to max
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    void TakeDamage()
    {
        currentHP--;

        // Die when HP reaches zero
        if (currentHP == 0)
        {
            Die();
        }
    }

    void Die()
    {

    }
}
