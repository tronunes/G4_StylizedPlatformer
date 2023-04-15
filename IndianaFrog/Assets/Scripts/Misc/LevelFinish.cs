using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the Player collides with this object")]
    public string nextScene;

    [Tooltip("Used for saving Player's progress")]
    public int currentLevelNumber;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerCheckpointHandler>().FinishLevel(nextScene, currentLevelNumber);
        }
    }
}
