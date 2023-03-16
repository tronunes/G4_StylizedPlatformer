using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private SphereCollider groundCheckCollider;
    [SerializeField] private PlayerMovement playerMovement;

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            playerMovement.SetGroundedState(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            playerMovement.SetGroundedState(false);
        }
    }
}
