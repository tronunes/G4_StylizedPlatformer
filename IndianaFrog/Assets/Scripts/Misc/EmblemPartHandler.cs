using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script has to be on the same object which has the animator
public class EmblemPartHandler : MonoBehaviour
{
    public EmblemDoor emblemDoor;

    public void StartOpeningDoor()
    {
        emblemDoor.OpenDoor();
    }
}
