using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script has to be on the same object which has the animator
public class EmblemDoorPartsHandler : MonoBehaviour
{
    public EmblemDoor emblemDoor;
    public GameObject emblemFuseParticles;
    public GameObject emblemHoverParticles;

    public void StartOpeningDoor()
    {
        emblemDoor.OpenDoor();
    }

    public void ActivateEmblemFuseParticles()
    {
        emblemFuseParticles.SetActive(true);
    }

    public void ActivateEmblemhoverParticles()
    {
        emblemHoverParticles.SetActive(true);
    }
}
