using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script has to be on the same object which has the animator
public class EmblemDoorPartsHandler : MonoBehaviour
{
    public EmblemDoor emblemDoor;
    public GameObject emblemFuseParticles;
    public GameObject emblemHoverParticles;
    public AudioSource doorActivates;
    public AudioSource doorHasActivated;

    public void StartOpeningDoor()
    {
        emblemDoor.OpenDoor();
    }

    public void ActivateEmblemFuseParticles()
    {
        emblemFuseParticles.SetActive(true);
        doorHasActivated.Play();
    }

    public void ActivateEmblemhoverParticles()
    {
        emblemHoverParticles.SetActive(true);
        doorActivates.Play();
    }
}
