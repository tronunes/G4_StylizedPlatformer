using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TongueEnd : MonoBehaviour
{
    private GrapplingTongueLauncher launcher;
    private Rigidbody tongueRb;
    public GameObject splashParticlesPrefab;
    public Transform splashParticlesSpawnPoint;

    void Start()
    {
        tongueRb = gameObject.GetComponent<Rigidbody>();

        // Orient the Tongue correctly
        transform.LookAt(transform.position + tongueRb.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Case: Tongue hit surface onto which it can latch
        if (collision.collider.gameObject.GetComponent<GrapplingStickySurface>())
        {
            // Prevent further collisions
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            // Latch onto the target
            transform.SetParent(collision.collider.transform.parent, true);

            // Orient the tongue to face the surface
            transform.LookAt(transform.position - collision.contacts[0].normal);

            // Start reeling the Frog
            launcher.StartReeling();

            // Activate splash particles
            Instantiate(splashParticlesPrefab, splashParticlesSpawnPoint.position, transform.rotation);
        }
        // Case: Tongue hit surface to which it doesn't stick
        else
        {
            launcher.RetractTongue();
        }
    }

    public void SetLauncher(GrapplingTongueLauncher givenLauncher)
    {
        launcher = givenLauncher;
    }

    public GrapplingTongueLauncher GetLauncher()
    {
        return launcher;
    }

    void FixedUpdate()
    {
        // Prevent the Tongue from going through surfaces when velocity is very high and surface very thin
        RaycastHit hit;
        if (Physics.Raycast(transform.position, tongueRb.velocity, out hit, tongueRb.velocity.magnitude * Time.fixedDeltaTime))
        {
            if (!hit.collider.isTrigger)
            {
                tongueRb.velocity = Vector3.ClampMagnitude(tongueRb.velocity, hit.distance / Time.fixedDeltaTime);
            }
        }
    }
}
