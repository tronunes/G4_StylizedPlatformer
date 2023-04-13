using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmblemHandler : MonoBehaviour
{
    private int emblemsCollected = 0;
    private int requiredEmblemCount = 3;

    [Header("Technical")]
    [SerializeField] private Transform emblemsUIWrapper;


    public void CollectEmblem()
    {
        emblemsCollected++;

        UpdateEmblemsUI();
    }

    void UpdateEmblemsUI()
    {
        // Activate the "Active" icons and disable "Inactive" icons for all collected emblems
        for (int i = 0; i < emblemsCollected; i++)
        {
            Transform emblemIcon = emblemsUIWrapper.Find("Emblem" + (i + 1).ToString());
            emblemIcon.Find("Active").gameObject.SetActive(true);
            emblemIcon.Find("Inactive").gameObject.SetActive(false);
        }
    }

    public bool CanDoorBeOpened()
    {
        return emblemsCollected >= requiredEmblemCount;
    }
}
