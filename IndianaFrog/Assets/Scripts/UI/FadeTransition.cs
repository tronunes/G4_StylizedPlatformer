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
    public AnimationCurve transitionCurve;
    private bool doFade = false;
    private bool doUnFade = false;

    void Start()
    {
        fadeImage = gameObject.GetComponent<Image>();
        originalColor = fadeImage.color;
        transitionTimer = 0f;
        UnFade();
    }

    void Update()
    {
        // DEBUG: remove these when not needed anymore for debugging
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fade();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            UnFade();
        }

        float imageAlpha = fadeImage.color.a;
        if (doFade)
        {
            // Lerp to fully black
            imageAlpha = transitionCurve.Evaluate(Mathf.Lerp(0f, 1f, 1f - transitionTimer / transitionDuration));
        }
        else if (doUnFade)
        {
            // Lerp to fully transparent
            imageAlpha = transitionCurve.Evaluate(Mathf.Lerp(0f, 1f, transitionTimer / transitionDuration));
        }

        // Reduce transition timer
        transitionTimer -= Time.deltaTime;
        if (transitionTimer < 0f)
        {
            // Make sure image alpha is either 1 or 0 when timer has run out
            if (doFade)
            {
                imageAlpha = 1f;
            }
            else if (doUnFade)
            {
                imageAlpha = 0f;
            }

            transitionTimer = 0f;
            doUnFade = false;
            doFade = false;
        }

        SetImageAlpha(imageAlpha);
    }

    // Transition from black to transparent
    public void UnFade()
    {
        // Unfade only if currently fully faded
        if (fadeImage.color.a == 1f)
        {
            transitionTimer = transitionDuration;
            doUnFade = true;
        }
    }

    // Transition from tranparent to black
    public void Fade()
    {
        // Fade only if currently fully unfaded
        if (fadeImage.color.a == 0f)
        {
            doFade = true;
            transitionTimer = transitionDuration;
        }
    }

    public void FadeAndUnFade()
    {

    }

    private void SetImageAlpha(float alpha)
    {
        fadeImage.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            alpha
        );
    }
}
