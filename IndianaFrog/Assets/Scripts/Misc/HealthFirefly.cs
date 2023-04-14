using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFirefly : MonoBehaviour
{
    public Transform fireflyMesh;
    private Vector3 startPosition;

    [Tooltip("How frequently the Firefly changes directions")] public float oscillateFrequency = 4;
    [Tooltip("Firefly movement radius")] [Range(0f, 1f)] public float movementRadius = 0.5f;
    private float randomOffset;


    void Start()
    {
        startPosition = fireflyMesh.transform.position;

        // Create a random offset for the Perlin value
        // to prevent multiple Fireflies having the same pattern
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // The "10f", "200f" and "3000f" are arbitrary. They are supposed to be different to produce different Perlin values.
            fireflyMesh.transform.position = startPosition +
                (-Vector3.one + 2f * new Vector3(
                    Mathf.PerlinNoise(Time.time * oscillateFrequency + randomOffset, 10f + randomOffset),
                    Mathf.PerlinNoise(Time.time * oscillateFrequency + randomOffset, 200f + randomOffset),
                    Mathf.PerlinNoise(Time.time * oscillateFrequency + randomOffset, 3000f + randomOffset))
                ) * movementRadius;
    }
}
