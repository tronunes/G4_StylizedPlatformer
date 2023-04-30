using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmblemDoor : MonoBehaviour
{
    private bool isOpen = false;
    private bool emblemsInserted = false;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform animatedEmblemParts; // Wrapper for the animated Emblem parts which fuse to the door
    [SerializeField] private GameObject dustParticles;
    public AudioSource doorOpens;


    void Start()
    {
        // Hide animated Emblems at Start
        animatedEmblemParts.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        // Case: Player enters the Door's trigger volume
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerEmblemHandler playerEmblemHandler = collider.gameObject.GetComponent<PlayerEmblemHandler>();

            // Check if the Player has enough Emblems collected
            if (playerEmblemHandler.CanDoorBeOpened())
            {
                // Begin animating Emblems' insertion to the Door
                InsertEmblems();

                // Hide the Player's circling emblems
                playerEmblemHandler.HideCirclingEmblems();
            }
        }
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorAnimator.SetTrigger("OpenDoor");

            // Play dust particles
            dustParticles.SetActive(true);
            doorOpens.Play();
        }
    }

    void InsertEmblems()
    {
        if (!emblemsInserted)
        {
            emblemsInserted = true;

            // Show emblems
            animatedEmblemParts.gameObject.SetActive(true);

            // Insert each emblem to the door via an animation
            animatedEmblemParts.GetComponent<Animator>().SetTrigger("StartFusing");
        }
    }

}
