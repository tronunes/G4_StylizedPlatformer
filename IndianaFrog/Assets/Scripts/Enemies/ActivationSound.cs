using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationSound : MonoBehaviour
{
    public AudioSource activationSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TotemActivationSound() 
    {
        if ( !activationSound.isPlaying )
        {
            activationSound.Play();
        }
    }

}
