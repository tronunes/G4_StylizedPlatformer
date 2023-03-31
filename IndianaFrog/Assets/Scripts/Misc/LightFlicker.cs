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


    void Start()
    {
        lightData = gameObject.GetComponent<HDAdditionalLightData>();;
        originalLightIntensity = lightData.intensity;
    }

    void Update()
    {
        // Change the light intensity over time with Perlin noise
        if (flickerPercentage > 0)
        {
            float dimmingValue = flickerPercentage * Mathf.PerlinNoise(Time.time * flickerFrequency, 0.0f);
            lightData.intensity = originalLightIntensity - originalLightIntensity * dimmingValue;
        }
    }
}