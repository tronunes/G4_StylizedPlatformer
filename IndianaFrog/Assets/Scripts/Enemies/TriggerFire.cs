using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFire : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    public AudioSource fireLaunchAudio;

    public void TriggerFiring()
    {
        enemy.Fire();
    }

    public void FireLaunchAudio()
    {
        fireLaunchAudio.Play();
    }
}
