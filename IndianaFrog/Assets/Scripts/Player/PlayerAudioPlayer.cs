using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioPlayer : MonoBehaviour
{
    public AudioSource footStep01;
    public Animator animator;
    private bool isWalking;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void eventFootStep()
    {
        footStep01.Play();
    }
}
