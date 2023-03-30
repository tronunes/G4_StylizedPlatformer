using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private int maxHP = 3;
    private int currentHP;

    // Checkpoints and respawning
    private Checkpoint latestCheckpoint;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        currentHP = maxHP;
    }

    void Update()
    {
        // Die when fell out of the map
        if (transform.position.y < -10f)
        {
            Die();
        }
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
        // Respawn
        transform.position = GetRespawnPosition();
        transform.rotation = GetRespawnRotation();
        ResetPlayer();
    }

    public void SetNewCheckpoint(Checkpoint newCheckpoint)
    {
        // Case: a checkpoint is already saved
        if (latestCheckpoint)
        {
            // Check if the newCheckpoint is further in the level
            if (newCheckpoint.checkpointIndex > latestCheckpoint.checkpointIndex)
            {
                latestCheckpoint = newCheckpoint;
            }
        }
        // Case: no previous checkpoints
        else
        {
            latestCheckpoint = newCheckpoint;
        }
    }

    private Vector3 GetRespawnPosition()
    {
        if (latestCheckpoint)
        {
            return latestCheckpoint.GetRespawnPosition();
        }
        else
        {
            return startPosition;
        }
    }

    private Quaternion GetRespawnRotation()
    {
        if (latestCheckpoint)
        {
            return latestCheckpoint.GetRespawnRotation();
        }
        else
        {
            return startRotation;
        }
    }

    void ResetPlayer()
    {
        // This component
        currentHP = maxHP;

        // Other components
        gameObject.GetComponent<PlayerMovement>().ResetPlayerMovement();
        gameObject.GetComponent<PlayerCameraController>().ResetPlayerCamera();
        gameObject.GetComponent<GrapplingTongueLauncher>().ResetTongueLauncher();
    }
}
