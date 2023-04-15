using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFirefly : MonoBehaviour
{
    public Transform fireflyMesh;
    public Transform parentTransfrom; // The topmost object of the Firefly hierarchy

    [Tooltip("How frequently the Firefly changes directions")] public float oscillateFrequency = 4;
    [Tooltip("Firefly movement radius")] [Range(0f, 1f)] public float movementRadius = 0.5f;
    private float randomOffset;
    private bool isBeingReeledIn = false; // When true, the Firefly has been hit by a tongue and is being reeled in
    private Transform tongueTransform; // The tongue, which reels the Firefly in


    void Start()
    {
        // Create a random offset for the Perlin value
        // to prevent multiple Fireflies having the same pattern
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Case: being reeled in by a Tongue
        if (isBeingReeledIn && tongueTransform != null)
        {
            fireflyMesh.transform.position = tongueTransform.position;
        }
        // Case: Normal Firefly life
        else
        {
            // The "10f", "200f" and "3000f" are arbitrary. They are supposed to be different to produce different Perlin values.
            fireflyMesh.transform.position = parentTransfrom.position + Vector3.up +
                (-Vector3.one + 2f * new Vector3(
                    Mathf.PerlinNoise(Time.time * oscillateFrequency + randomOffset, 10f + randomOffset),
                    Mathf.PerlinNoise(Time.time * oscillateFrequency + randomOffset, 200f + randomOffset),
                    Mathf.PerlinNoise(Time.time * oscillateFrequency + randomOffset, 3000f + randomOffset))
                ) * movementRadius;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // Case: being reeled in
        if (isBeingReeledIn)
        {
            PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Increase the Player's HP and then destory self
                playerHealth.AddHealth(1);

                // Disable the Firefly
                DisableFirefly();
            }
        }
        // Case: NOT being reeled in
        else
        {
            // Case: a Tongue hit the Firefly
            TongueEnd tongue = collider.gameObject.GetComponent<TongueEnd>();
            if (tongue != null)
            {
                // Start reeling the Firefly towards the Frog
                GrapplingTongueLauncher launcher = tongue.GetLauncher();
                launcher.RetractTongue();
                isBeingReeledIn = true;
                tongueTransform = tongue.transform;

                // Disable the Firefly's collider
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void RespawnFirefly()
    {
        fireflyMesh.gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
        isBeingReeledIn = false;
        tongueTransform = null;
    }

    private void DisableFirefly()
    {
        fireflyMesh.gameObject.SetActive(false);
    }
}
