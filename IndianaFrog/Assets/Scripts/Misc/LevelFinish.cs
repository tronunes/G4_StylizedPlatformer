using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the Player collides with this object")]
    public string nextScene;

    [Tooltip("Used for saving Player's progress")]
    public int currentLevelNumber;

    public AudioSource levelFinish;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //collider.gameObject.GetComponent<PlayerCheckpointHandler>().FinishLevel(nextScene, currentLevelNumber);
            StartCoroutine(StageEndCoroutine(collider));
        }
    }

    IEnumerator StageEndCoroutine(Collider collider)
    {
        if ( !levelFinish.isPlaying )
        {
        levelFinish.Play();
        yield return new WaitForSeconds( levelFinish.clip.length - 3.18f);

        collider.gameObject.GetComponent<PlayerCheckpointHandler>().FinishLevel(nextScene, currentLevelNumber);
        }
    }
}
