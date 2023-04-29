using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFootDustParticles : MonoBehaviour
{
    public ParticleSystem leftFootDust;
    public ParticleSystem rightFootDust;

    public void SpawnLeftFootDust()
    {
        leftFootDust.Play();
    }

    public void SpawnRightFootDust()
    {
        rightFootDust.Play();
    }
}
