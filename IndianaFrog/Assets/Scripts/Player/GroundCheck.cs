using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private SphereCollider groundCheckCollider;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int overlaps = 0;

    void OnTriggerEnter(Collider collider)
    {
        // Grounded
        if (!collider.gameObject.CompareTag("Player") && !collider.isTrigger)
        {
            overlaps++;
            if (overlaps > 0)
            {
                playerMovement.SetGroundedState(true);
            }

            // Parent Character to Platform
            if (collider.gameObject.CompareTag("Platform"))
            {
                // Use the parent of the collider as the parent, so that the collider mesh can be scaled however we want.
                // The parent's scale needs to be (1,1,1) in order not to skew the Frog
                playerMovement.ParentToPlatform(collider.transform.parent);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Ungrounded (i.e. not touching ground)
        if (!collider.gameObject.CompareTag("Player") && !collider.isTrigger)
        {
            if (overlaps > 0)
            {
                overlaps--;
            }
            else
            {
                overlaps = 0;
            }

            if (overlaps == 0)
            {
                playerMovement.SetGroundedState(false);

                // Unparent Character from Platform
                playerMovement.ClearParent();
            }
        }
    }

    public void ResetOverlaps()
    {
        overlaps = 0;
    }
}
