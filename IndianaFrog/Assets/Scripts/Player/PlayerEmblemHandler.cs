using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmblemHandler : MonoBehaviour
{
    private int emblemsCollected = 0;
    private int requiredEmblemCount = 3;

    [Header("Technical")]
    [SerializeField] private Transform emblemsUIWrapper;


    public void CollectEmblem(EmblemPartChoices partType)
    {
        emblemsCollected++;

        UpdateEmblemsUI(partType);
    }

    void UpdateEmblemsUI(EmblemPartChoices partType)
    {
        // Get the correct object name, for example "Emblem_Top"
        string nameSuffix = "";
        if (partType == EmblemPartChoices.TOP) { nameSuffix = "Top"; }
        else if (partType == EmblemPartChoices.RIGHT) { nameSuffix = "Right"; }
        else if (partType == EmblemPartChoices.BOTTOM) { nameSuffix = "Bottom"; }

        // Enable the "active" icon and disable the "inactive" icon for the Emblem
        Transform emblemIcon = emblemsUIWrapper.Find("Emblem_" + nameSuffix);
        emblemIcon.Find("Active").gameObject.SetActive(true);
        emblemIcon.Find("Inactive").gameObject.SetActive(false);
    }

    public bool CanDoorBeOpened()
    {
        return emblemsCollected >= requiredEmblemCount;
    }
}
