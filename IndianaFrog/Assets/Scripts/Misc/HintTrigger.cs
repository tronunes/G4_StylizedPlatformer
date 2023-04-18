using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public GameObject hintText;

    void Start()
    {
        HideHintText();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            ShowHintText();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            HideHintText();
        }
    }

    void ShowHintText()
    {
        hintText.SetActive(true);
    }

    void HideHintText()
    {
        hintText.SetActive(false);
    }
}
