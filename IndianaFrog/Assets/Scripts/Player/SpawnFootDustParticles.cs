using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnFootDustParticles : MonoBehaviour
{
    public ParticleSystem leftFootDust;
    public ParticleSystem rightFootDust;
    public AudioSource footStep;

    public void SpawnLeftFootDust()
    {
        leftFootDust.Play();
        footStep.Play();
    }

    public void SpawnRightFootDust()
    {
        rightFootDust.Play();
        footStep.Play();
    }
}
