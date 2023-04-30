using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioPlayer : MonoBehaviour
{
    private bool hasAttached = false;

    public Animator animator;

    public AudioSource frogJump;
    public AudioSource frogJumpCharge;
    public AudioSource frogLand;
    public AudioSource frogSlide;
    public AudioSource tongueLaunch;
    public AudioSource tongueReeling;
    public AudioSource tongueAttach;

    private void Start()
    {

    }

    private void Update()
    {
        PlayReeling();

        PlayCharging();   
    }

    private void DeathSlam() {}
    private void JumpStart() 
    {
        frogJump.Play();
    }
    private void JumpEnd() 
    {
        frogLand.Play();
    }    

    private void SlideStart() 
    {
        frogSlide.Play();
    }
    private void SlideEnd() {}
    private void GrappleStart() 
    {
        tongueLaunch.Play();
    }
    private void GrappleEnd() {}
    private void TakeDamage() {}

    private void PlayReeling()
    {
        if (animator.GetBool("Reeling"))
        {
            switch (tongueReeling.isPlaying)
            {
                case true:
                    break;
                case false:
                    if ( !hasAttached ) 
                    {
                        PlayAttach();
                        hasAttached = true;
                    }
                    tongueReeling.Play();
                    break;
            }
        }
        else
        {
            switch (tongueReeling.isPlaying)
            {
                case true:
                    tongueReeling.Stop();
                    hasAttached = false;
                    break;
                case false:
                    break;
            }
        }
    }

    private void PlayCharging() 
    {
        if (Input.GetButtonDown("Jump"))
        {
            frogJumpCharge.Play();
        } 
        else if (Input.GetButtonUp("Jump"))
        {
            frogJumpCharge.Stop();
        } else {}
    }

    private void PlayAttach()
    {
        switch ( hasAttached )
        {
            case true: 
                break;
            case false:
                tongueAttach.Play();
                break;
        }
    }

}
