using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightFlicker : MonoBehaviour
{
    private float originalLightIntensity;
    [Tooltip("How much the light dims when flickering")] [Range(0f, 1f)] public float dimmingPercentage;
    [Tooltip("How frequently the light flickers")] public float flickerFrequency = 1;
    private HDAdditionalLightData lightData;
    private Vector3 lightOriginalPosition;
    [Tooltip("Should the position of the lightsource also oscillate randomly")] public bool oscillateLightPosition = false;
    [Tooltip("Lightsource position movement radius")] [Range(0f, 0.2f)] public float lightMovementRadius = 0.05f;
    [Tooltip("I.e. randomize flickering start position")] public bool useRandomOffset = true;
    private float randomOffset;
    [Tooltip("How long it takes the light to \"warm up\"")] public float buildUpTime = 1f;
    private float buildUpTimeLeft;


    void Start()
    {
        lightData = gameObject.GetComponent<HDAdditionalLightData>();;
        originalLightIntensity = lightData.intensity;
        lightData.intensity = 0f;
        buildUpTimeLeft = buildUpTime;

        lightOriginalPosition = transform.position;

        // Create a random offset for the Perlin value
        // to prevent multiple lights having the same pattern
        randomOffset = useRandomOffset ? Random.Range(0f, 100f) : 0f;
    }

    void Update()
    {
        // Case: Build-up
        if (buildUpTimeLeft > 0f)
        {
            lightData.intensity = originalLightIntensity * (1f - buildUpTimeLeft / buildUpTime);
            buildUpTimeLeft -= Time.deltaTime;
        }
        // Case: normal light flickering
        else
        {
            // Change the light intensity over time with Perlin noise
            float dimmingValue = dimmingPercentage * Mathf.PerlinNoise(Time.time * flickerFrequency + randomOffset, 0.0f + randomOffset);
            lightData.intensity = originalLightIntensity - originalLightIntensity * dimmingValue;
        }

        // Change the lightsource position over time with Perlin noise
        if (oscillateLightPosition)
        {
            // The "10f", "20f" and "30f" are arbitrary. They are supposed to be different to produce different Perlin values.
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