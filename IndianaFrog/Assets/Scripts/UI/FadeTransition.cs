using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeTransition : MonoBehaviour
{
    private Image fadeImage;
    private Color originalColor;
    private float transitionTimer = 0f; // The current position in the transition
    public float transitionDuration = 1.5f; // The total duration of the transition
    public AnimationCurve transitionCurve;
    private bool doFade = false;
    private bool doUnFade = false;

    [Header("Events")]
    public UnityEvent event_FadeFinished = new UnityEvent();
    public UnityEvent event_UnfadeFinished = new UnityEvent();


    void Start()
    {
        fadeImage = gameObject.GetComponent<Image>();
        originalColor = fadeImage.color;
        transitionTimer = 0f;
        UnFade();
    }

    void Update()
    {
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

        // Case: fade / unfade complete
        if (transitionTimer < 0f)
        {
            // Make sure image alpha is either 1 or 0 when timer has run out
            if (doFade)
            {
                // Reset fading values before invoking the event
                ResetFadingValues();

                // Update the fading image transparency
                imageAlpha = 1f;
                SetImageAlpha(imageAlpha);

                // Invoke event
                event_FadeFinished.Invoke();
            }
            else if (doUnFade)
            {
                // Reset fading values before invoking the event
                ResetFadingValues();

                // Update the fading image transparency
                imageAlpha = 0f;
                SetImageAlpha(imageAlpha);

                // Invoke event
                event_UnfadeFinished.Invoke();
            }
        }

        // Update the fading image transparency when either fading or unfading
        if (doFade || doUnFade)
        {
            SetImageAlpha(imageAlpha);
        }
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

    void ResetFadingValues()
    {
        transitionTimer = 0f;
        doUnFade = false;
        doFade = false;
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
