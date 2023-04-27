using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeath : MonoBehaviour
{
    public PlayerCheckpointHandler checkpointHandler;

    public void KillPlayer()
    {
        // Reset the player to the most recent checkpoint
        checkpointHandler.Die();
    }
}
