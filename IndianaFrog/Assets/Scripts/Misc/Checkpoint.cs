using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;

    void OnDrawGizmos()
    {
        // Draw the trigger volume in editor
        // =================================

        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();

        Gizmos.color = new Color(1f, 0.92f, 0.016f, 1f); // Yellow
        Gizmos.DrawWireCube(
            transform.position + boxCollider.center,
            boxCollider.size
        );
    }
}
