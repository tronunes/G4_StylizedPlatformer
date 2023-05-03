using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmblemHandler : MonoBehaviour
{
    private int emblemsCollected = 0;
    private int requiredEmblemCount = 3;

    [Header("Technical")]
    [SerializeField] private Transform emblemsUIWrapper;
    [SerializeField] private GameObject circlingEmblemTop;
    [SerializeField] private GameObject circlingEmblemRight;
    [SerializeField] private GameObject circlingEmblemBottom;

    public AudioSource emblemCollectSound;

    void Start()
    {
        HideCirclingEmblems();
    }


    public void CollectEmblem(EmblemPartChoices partType)
    {
        emblemsCollected++;

        UpdateEmblemsUI(partType);
        UpdateCirclingEmblems(partType);
        emblemCollectSound.Play();
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

    void UpdateCirclingEmblems(EmblemPartChoices partType)
    {
        if (partType == EmblemPartChoices.TOP)
        {
            circlingEmblemTop.SetActive(true);
        }
        else if (partType == EmblemPartChoices.RIGHT)
        {
            circlingEmblemRight.SetActive(true);
        }
        else if (partType == EmblemPartChoices.BOTTOM)
        {
            circlingEmblemBottom.SetActive(true);
        }
    }

    public void HideCirclingEmblems()
    {
        circlingEmblemTop.SetActive(false);
        circlingEmblemRight.SetActive(false);
        circlingEmblemBottom.SetActive(false);
    }

    public bool CanDoorBeOpened()
    {
        return emblemsCollected >= requiredEmblemCount;
    }
}
