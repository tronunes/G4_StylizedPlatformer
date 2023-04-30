using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherManager : MonoBehaviour
{
    private GameObject hpFeather0;
    private GameObject hpFeather1;
    private GameObject hpFeather2;

    [Header("FEATHER BLINKING")]
    [Tooltip("How many times feather blinks")]
    public int featherBlinkCount = 3; //how many times feather blinks
    public float blinkSpeed = 0.1f;

    void Awake()
    {
        hpFeather0 = GameObject.Find("hpFeather/hp0");
        hpFeather1 = GameObject.Find("hpFeather/hp1");
        hpFeather2 = GameObject.Find("hpFeather/hp2");
    }

    public void AddFeather(int currentHealth, string eventName)
    {
        FeatherChange(currentHealth, eventName);
    }

    public void RemoveFeather(int currentHealth, string eventName)
    {
        FeatherChange(currentHealth, eventName);
    }

    public void FeatherReset()
    {
        hpFeather0.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        hpFeather1.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        hpFeather2.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }

    private void FeatherChange(int currentHealth, string eventName)
    {
        switch (currentHealth)
        {
            case 3:
                StartCoroutine(BlinkFeather(hpFeather2, eventName));
                break;
            case 2:
                StartCoroutine(BlinkFeather(hpFeather1, eventName));
                break;
            case 1:
                StartCoroutine(BlinkFeather(hpFeather0, eventName));
                break;
        }
    }

    private IEnumerator BlinkFeather(GameObject feather, string eventName)
    {
        if (feather != null)
        {
            SkinnedMeshRenderer currentFeatherRenderer = feather.GetComponentInChildren<SkinnedMeshRenderer>();

            //Player lost hp
            if (eventName.Equals("SubtractHealth"))
            {
                //handle blinking current feather
                for (int i = 0; i < featherBlinkCount; i++)
                {
                    currentFeatherRenderer.enabled = (false);
                    yield return new WaitForSeconds(blinkSpeed);

                    currentFeatherRenderer.enabled = (true);
                    yield return new WaitForSeconds(blinkSpeed);
                }
                // Hide feather until gained hp or reset
                currentFeatherRenderer.enabled = (false);

            } //Player gained hp
            else if (eventName.Equals("AddHealth"))
            {
                //handle blinking current feather
                for (int i = 0; i < featherBlinkCount; i++)
                {
                    currentFeatherRenderer.enabled = (true);
                    yield return new WaitForSeconds(blinkSpeed);

                    currentFeatherRenderer.enabled = (false);
                    yield return new WaitForSeconds(blinkSpeed);
                }
                // Reveal feather until next damage
                currentFeatherRenderer.enabled = (true);
            }
        }
    }
}
