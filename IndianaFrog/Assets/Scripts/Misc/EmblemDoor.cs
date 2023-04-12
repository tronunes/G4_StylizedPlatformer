using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmblemDoor : MonoBehaviour
{
    private bool isOpen = false;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Transform emblemParts;


    void Start()
    {
        // Hide emblems at Start
        emblemParts.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject.GetComponent<PlayerEmblemHandler>().CanDoorBeOpened())
            {
                InsertEmblems();
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorAnimator.SetTrigger("OpenDoor");
        }
    }

    void InsertEmblems()
    {
        // Show emblems
        emblemParts.gameObject.SetActive(true);

        // Insert each emblem to the door via an animation
        foreach(Transform emblemPart in emblemParts)
        {
            emblemPart.GetComponent<Animator>().SetTrigger("Move");
        }
    }
}
