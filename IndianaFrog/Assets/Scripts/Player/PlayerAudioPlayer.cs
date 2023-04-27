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

    private void DeathSlam() {}
    private void JumpStart() {}
    private void JumpEnd() {}    
    private void FootStep() 
    {
        footStep01.Play();
    }

    private void SlideStart() {}
    private void SlideEnd() {}
    private void GrappleStart() {}
    private void GrappleEnd() {}
    private void TakeDamage() {}
}
