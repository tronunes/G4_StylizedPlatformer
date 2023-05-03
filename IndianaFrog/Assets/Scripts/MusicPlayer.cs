using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [Range(0, 1f)]
    public float songVolume = 1f;
    public AudioSource song;
    
    void Awake()
    {
        song.volume = songVolume;
        song.Play();
    }

}
