using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private SphereCollider groundCheckCollider;
    [SerializeField] private PlayerMovement playerMovement;

    void OnTriggerEnter(Collider collider)
    {
        // Grounded
        if (!collider.gameObject.CompareTag("Player"))
        {
            playerMovement.SetGroundedState(true);
        }

        // Parenting to Platform
        if (collider.gameObject.CompareTag("Platform"))
        {
            playerMovement.ParentToPlatform(collider.transform);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Ungrounded
        if (!collider.gameObject.CompareTag("Player"))
        {
            playerMovement.SetGroundedState(false);
        }

        // Unparent from Platform
        if (collider.gameObject.CompareTag("Platform"))
        {
            playerMovement.ClearParent();
        }
    }
}
