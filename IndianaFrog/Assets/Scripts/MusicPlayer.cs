using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [Range(0, 1f)]
    public float songVolume = 1f;
    public AudioSource songMainMenu;
    public AudioSource songLevel1;
    public AudioSource songLevel2;
    
    void Awake()
    {
        SetVolume();
        switch ( SceneManager.GetActiveScene().name )
        {
            case "Main Menu":
                songMainMenu.Play();
                break;
            case "Level 1":
                songLevel1.Play();
                break;
            case "Level 2":
                songLevel2.Play();
                break;
        }
    }

    void SetVolume()
    {
        songMainMenu.volume = songVolume;
        songLevel1.volume = songVolume;
        songLevel2.volume = songVolume;
    }
}
