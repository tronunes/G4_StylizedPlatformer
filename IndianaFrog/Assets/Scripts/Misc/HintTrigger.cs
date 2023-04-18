using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public GameObject hintText;
    public Animator hintAnimator;

    void Start()
    {
        // Start as disabled to make the initial hiding invisible
        hintText.SetActive(false);
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
        hintAnimator.SetTrigger("ShowHint");
    }

    void HideHintText()
    {
        hintAnimator.SetTrigger("HideHint");
    }
}
