using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpointHandler : MonoBehaviour
{
    // Checkpoints and respawning
    private Checkpoint latestCheckpoint;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Die()
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
        // Other components
        gameObject.GetComponent<PlayerMovement>().ResetPlayerMovement();
        gameObject.GetComponent<PlayerCameraController>().ResetPlayerCamera();
        gameObject.GetComponent<GrapplingTongueLauncher>().ResetTongueLauncher();
        gameObject.GetComponent<PlayerHealth>().ResetHealth();
    }
}