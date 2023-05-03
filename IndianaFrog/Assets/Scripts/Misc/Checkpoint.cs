using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;
    public int checkpointIndex; // Indicates how far in the level the checkpoint is. A higher index checkpoint overrides a lower index.
    [SerializeField] private GameObject fireEffect;
    public AudioSource fireLoop;
    public AudioSource fireActivate;

    private bool isActive = false;


    void Awake()
    {
        fireEffect.SetActive(false);
        fireLoop.Stop();
        isActive = false;
    }

    void OnDrawGizmos()
    {
        // Draw the trigger volume in editor
        // =================================

        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();

        Gizmos.color = new Color(1f, 0.92f, 0.016f, 1f); // Yellow
        Gizmos.DrawWireCube(
            transform.position + boxCollider.center,
            boxCollider.size * transform.localScale.x
        );
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerCheckpointHandler checkpointHandler = collider.gameObject.GetComponent<PlayerCheckpointHandler>();
            checkpointHandler.SetNewCheckpoint(this);

            // Activate the fire
            fireEffect.SetActive(true);
            fireLoop.Play();
            if ( !(isActive) ) 
            {
                fireActivate.Play();
                isActive = true;
            }
        }
    }

    public Vector3 GetRespawnPosition()
    {
        return respawnTransform.position;
    }

    public Quaternion GetRespawnRotation()
    {
        return respawnTransform.rotation;
    }
}
