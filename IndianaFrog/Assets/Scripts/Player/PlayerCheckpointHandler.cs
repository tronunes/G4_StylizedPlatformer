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
    private string nextLevelName;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Die()
    {
        EnableOrDisablePlayerInput(false);

        // Start fading
        fadeTransitionHandler.event_FadeFinished.AddListener(Respawn);
        fadeTransitionHandler.Fade();
    }

    public void FinishLevel(string levelName)
    {
        EnableOrDisablePlayerInput(false);

        // Start fading
        fadeTransitionHandler.event_FadeFinished.AddListener(() => {
            fadeTransitionHandler.event_FadeFinished.RemoveAllListeners();
            SceneManager.LoadScene(levelName);
        });
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
        gameObject.GetComponent<PlayerMovement>().ResetPlayerMovement();
        gameObject.GetComponent<PlayerCameraController>().ResetPlayerCamera();
        gameObject.GetComponent<GrapplingTongueLauncher>().ResetTongueLauncher();
        gameObject.GetComponent<PlayerHealth>().ResetHealth();
    }

    void Respawn()
    {
        // Remove the listener
        fadeTransitionHandler.event_FadeFinished.RemoveListener(Respawn);

        transform.position = GetRespawnPosition();
        transform.rotation = GetRespawnRotation();
        ResetPlayer();

        // Start unfading
        fadeTransitionHandler.event_UnfadeFinished.AddListener(() => 
        {
            EnableOrDisablePlayerInput(true);
            fadeTransitionHandler.event_UnfadeFinished.RemoveAllListeners();
        });
        fadeTransitionHandler.UnFade();
    }

    void EnableOrDisablePlayerInput(bool newEnabledState)
    {
        gameObject.GetComponent<PlayerMovement>().inputLocked = !newEnabledState;
        gameObject.GetComponent<PlayerCameraController>().inputLocked = !newEnabledState;
        gameObject.GetComponent<GrapplingTongueLauncher>().inputLocked = !newEnabledState;
    }
}
