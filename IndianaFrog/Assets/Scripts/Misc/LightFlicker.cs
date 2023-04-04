using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightFlicker : MonoBehaviour
{
    private float originalLightIntensity;
    [Range(0f, 1f)] public float flickerPercentage;
    public float flickerFrequency = 1;
    private HDAdditionalLightData lightData;
    private Vector3 lightOriginalPosition;
    public bool moveLightSource = false; // Moves the lightsource transform position with Perlin noise
    [Range(0f, 0.2f)] public float lightMovementRadius = 0.05f;


    void Start()
    {
        lightData = gameObject.GetComponent<HDAdditionalLightData>();;
        originalLightIntensity = lightData.intensity;

        lightOriginalPosition = transform.position;
    }

    void Update()
    {
        // Change the light intensity over time with Perlin noise
        if (flickerPercentage > 0)
        {
            float dimmingValue = flickerPercentage * Mathf.PerlinNoise(Time.time * flickerFrequency, 0.0f);
            lightData.intensity = originalLightIntensity - originalLightIntensity * dimmingValue;

            // Change the lightsource position over time with Perlin noise
            if (moveLightSource)
            {
                transform.position =
                    lightOriginalPosition +
                        (-Vector3.one + 2f * new Vector3(
                            Mathf.PerlinNoise(Time.time * flickerFrequency, 10f),
                            Mathf.PerlinNoise(Time.time * flickerFrequency, 20f),
                            Mathf.PerlinNoise(Time.time * flickerFrequency, 30f))
                        ) * lightMovementRadius;
            }
        }
    }
}