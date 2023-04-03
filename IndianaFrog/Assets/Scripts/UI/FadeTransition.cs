using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    private Image fadeImage;
    private Color originalColor;
    private float transitionTimer = 0f; // The current position in the transition
    public float transitionDuration = 1.5f; // The total duration of the transition
    private bool doFade = false;
    private bool doUnFade = false;

    void Start()
    {
        fadeImage = gameObject.GetComponent<Image>();
        originalColor = fadeImage.color;
        transitionTimer = 0f;
        UnFade();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fade();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            UnFade();
        }

        if (doFade)
        {
            // Lerp between fully black and fully transparent by the transitionTimer
            fadeImage.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(0f, 1f, transitionTimer / transitionDuration)
            );
        }
        else if (doUnFade)
        {
            // Lerp between fully black and fully transparent by the transitionTimer
            fadeImage.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(0f, 1f, 1f - transitionTimer / transitionDuration)
            );
        }

        // Reduce transition timer
        transitionTimer -= Time.deltaTime;
        if (transitionTimer < 0f)
        {
            transitionTimer = 0f;
            doUnFade = false;
            doFade = false;
        }
    }

    // Transition from black to transparent
    public void UnFade()
    {
        transitionTimer = transitionDuration;
        doUnFade = true;
    }

    // Transition from tranparent to black
    public void Fade()
    {
        doFade = true;
        transitionTimer = transitionDuration;
    }

    public void FadeAndUnFade()
    {

    }
}
