using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherManager : MonoBehaviour
{
    [Header("FEATHER DAMAGE")]
    public int blinksFeather = 3; //how many times feather blinks
    public float blinkSpeed = 0.1f;

    //Selected feather based on hp that script will act on
    private GameObject currentFeather;
    private SkinnedMeshRenderer currentFeatherRenderer;
    private GameObject hpFeather0;
    private GameObject hpFeather1;
    private GameObject hpFeather2;

    void Awake()
    {
        hpFeather0 = GameObject.Find("hpFeather/hp0");
        hpFeather1 = GameObject.Find("hpFeather/hp1");
        hpFeather2 = GameObject.Find("hpFeather/hp2");
        currentFeather = hpFeather2;
    }

    public void AddFeather(int currentHealth, string eventName)
    {
        StartCoroutine(FeatherChange(currentHealth, eventName));
    }

    public void RemoveFeather(int currentHealth, string eventName)
    {
        StartCoroutine(FeatherChange(currentHealth, eventName));
    }

    public void FeatherReset()
    {
        hpFeather0.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        hpFeather1.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        hpFeather2.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        currentFeather = hpFeather2;
    }

    public IEnumerator FeatherChange(int currentHealth, string eventName)
    {
        switch (currentHealth)
        {
            case 3:
                currentFeather = hpFeather2;
                break;
            case 2:
                currentFeather = hpFeather1;
                break;
            case 1:
                currentFeather = hpFeather0;
                break;
            default:
                Debug.Log("Something went wrong with getting current health");
                currentFeather = null;
                break;
        }

        if (currentFeather != null)
        {
            currentFeatherRenderer = currentFeather.GetComponentInChildren<SkinnedMeshRenderer>();

            if (eventName.Equals("SubtractHealth"))
            {
                //handle blinking in feather
                for (int i = 0; i < blinksFeather; i++)
                {

                    currentFeatherRenderer.enabled = (false);
                    yield return new WaitForSeconds(blinkSpeed);

                    currentFeatherRenderer.enabled = (true);
                    yield return new WaitForSeconds(blinkSpeed);
                }
                // Hide the object until reset
                currentFeatherRenderer.enabled = (false);

            }
            else if (eventName.Equals("AddHealth"))
            {
                //handle blinking
                for (int i = 0; i < blinksFeather; i++)
                {
                    // Hide the object
                    currentFeatherRenderer.enabled = (true);
                    yield return new WaitForSeconds(blinkSpeed);

                    currentFeatherRenderer.enabled = (false);
                    yield return new WaitForSeconds(blinkSpeed);
                }
                // Hide the object until reset
                currentFeatherRenderer.enabled = (true);
            }
        }

    }
}
