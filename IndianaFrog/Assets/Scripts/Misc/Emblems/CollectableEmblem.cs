using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// GLOBAL: Choices for the visible part model
public enum EmblemPartChoices
{
    TOP,
    RIGHT,
    BOTTOM
}

public class CollectableEmblem : MonoBehaviour
{
    public EmblemPartChoices visiblePartModel;
    public GameObject collectParticlesPrefab; // Prefab of the particle effect which is spawned when the Emblem is collected

    void OnTriggerEnter(Collider collider)
    {
        // Case: Player collides with the Emblem
        if (collider.gameObject.CompareTag("Player"))
        {
            // Collect the Emblem
            collider.gameObject.GetComponent<PlayerEmblemHandler>().CollectEmblem(visiblePartModel);

            // Activate particles
            Instantiate(collectParticlesPrefab, transform.position + Vector3.up, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
