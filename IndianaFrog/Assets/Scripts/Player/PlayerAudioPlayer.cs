using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator animator;
    private bool isWalking;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool newWalkingState = animator.GetBool("Running");

        if (newWalkingState != isWalking)
        {
            isWalking = newWalkingState;

            if (isWalking)
            {
                audioSource.Play();
                audioSource.loop = true;
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
