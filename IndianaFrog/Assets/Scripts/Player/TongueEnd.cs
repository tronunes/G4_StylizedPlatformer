using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TongueEnd : MonoBehaviour
{
    private GrapplingTongueLauncher launcher;

    void OnCollisionEnter(Collision collision)
    {
        // Prevent further collisions
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        // Latch onto the target
        transform.SetParent(collision.collider.transform, true);

        // Orient the tongue to face the surface
        transform.LookAt(collision.contacts[0].point - collision.contacts[0].normal);

        // Start reeling the Frog
        launcher.StartReeling();
    }

    public void SetLauncher(GrapplingTongueLauncher givenLauncher)
    {
        launcher = givenLauncher;
    }
}
