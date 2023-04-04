using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightFlicker : MonoBehaviour
{
    private float originalLightIntensity;
    [Range(0f, 1f)] public float dimmingPercentage;
    public float flickerFrequency = 1;
    private HDAdditionalLightData lightData;
    private Vector3 lightOriginalPosition;
    public bool oscillateLightPosition = false; // Moves the lightsource transform position with Perlin noise
    [Range(0f, 0.2f)] public float lightMovementRadius = 0.05f;
    public bool useRandomOffset = true;
    private float randomOffset;


    void Start()
    {
        lightData = gameObject.GetComponent<HDAdditionalLightData>();;
        originalLightIntensity = lightData.intensity;

        lightOriginalPosition = transform.position;

        // Create a random offset for the Perlin value
        // to prevent multiple lights having the same pattern
        randomOffset = useRandomOffset ? Random.Range(0f, 100f) : 0f;
    }

    void Update()
    {
        // Change the light intensity over time with Perlin noise
        if (dimmingPercentage > 0)
        {
            float dimmingValue = dimmingPercentage * Mathf.PerlinNoise(Time.time * flickerFrequency + randomOffset, 0.0f + randomOffset);
            lightData.intensity = originalLightIntensity - originalLightIntensity * dimmingValue;

            // Change the lightsource position over time with Perlin noise
            if (oscillateLightPosition)
            {
                transform.position =
                    lightOriginalPosition +
                        (-Vector3.one + 2f * new Vector3(
                            Mathf.PerlinNoise(Time.time * flickerFrequency + randomOffset, 10f + randomOffset),
                            Mathf.PerlinNoise(Time.time * flickerFrequency + randomOffset, 20f + randomOffset),
                            Mathf.PerlinNoise(Time.time * flickerFrequency + randomOffset, 30f + randomOffset))
                        ) * lightMovementRadius;
            }
        }
    }
}