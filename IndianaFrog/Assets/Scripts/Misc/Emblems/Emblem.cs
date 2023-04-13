using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum EmblemPartChoices // Choices for the visible part model
{
    TOP,
    RIGHT,
    BOTTOM
}

public class Emblem : MonoBehaviour
{
    public EmblemPartChoices visiblePartModel;
    private Transform emblemPartsWrapper;
    public GameObject collectParticlesPrefab; // Prefab of the particle effect which is spawned when the Emblem is collected

    void Start()
    {
        ShowCorrectPartModel();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerEmblemHandler>().CollectEmblem(visiblePartModel);

            // Activate particles
            Instantiate(collectParticlesPrefab, transform.position + Vector3.up, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    void ShowCorrectPartModel()
    {
        emblemPartsWrapper = transform.Find("Animator").Find("EmblemParts");

        // Hide all part models
        foreach (Transform child in emblemPartsWrapper)
        {
            child.gameObject.SetActive(false);
        }

        // Activate the correct part model
        switch (visiblePartModel)
        {
            case EmblemPartChoices.TOP:
                emblemPartsWrapper.Find("EmblemPart_Top").gameObject.SetActive(true);
                break;
            case EmblemPartChoices.RIGHT:
                emblemPartsWrapper.Find("EmblemPart_Right").gameObject.SetActive(true);
                break;
            case EmblemPartChoices.BOTTOM:
                emblemPartsWrapper.Find("EmblemPart_Bottom").gameObject.SetActive(true);
                break;
        }
    }
}
