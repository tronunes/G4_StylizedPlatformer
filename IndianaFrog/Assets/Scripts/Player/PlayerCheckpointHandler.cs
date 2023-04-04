using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckpointHandler : MonoBehaviour
{
    // Checkpoints and respawning
    private Checkpoint latestCheckpoint;
    private Vector3 startPosition;
    private Quaternion startRotation;

    [SerializeField] private FadeTransition fadeTransitionHandler;
    private bool doRespawn = false;
    private bool doLoadNextLevel = false;
    private string nextLevelName;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Add a callback for fade finished event
        fadeTransitionHandler.event_FadeFinished.AddListener(FadeCallback);
    }

    public void Die()
    {
        doRespawn = true;

        // Start fading
        fadeTransitionHandler.Fade();
    }

    public void FinishLevel(string levelName)
    {
        doLoadNextLevel = true;
        nextLevelName = levelName;

        // Start fading
        fadeTransitionHandler.Fade();
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

    void Respawn()
    {
        transform.position = GetRespawnPosition();
        transform.rotation = GetRespawnRotation();
        ResetPlayer();

        // Start unfading
        fadeTransitionHandler.UnFade();
    }

    void FadeCallback()
    {
        if (doRespawn)
        {
            doRespawn = false;
            Respawn();
        }

        if (doLoadNextLevel)
        {
            doLoadNextLevel = false;
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
