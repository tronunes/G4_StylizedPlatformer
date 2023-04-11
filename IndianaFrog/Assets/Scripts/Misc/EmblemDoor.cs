using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmblemDoor : MonoBehaviour
{
    private bool isOpen = false;
    [SerializeField] Animator animator;


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject.GetComponent<PlayerEmblemHandler>().CanDoorBeOpened())
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetTrigger("OpenDoor");
        }
    }
}
