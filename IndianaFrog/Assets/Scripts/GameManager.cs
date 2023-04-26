using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

// A Singleton, prevent duplicates
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    //add serialized audio mixer here
    public AudioMixer audioMixer;

    private bool isPaused = false;

    //Input
    private float joystickXSensitivity = 1f;
    private float joystickYSensitivity = 1f;
    
    //Graphics Quality preset
    private int graphicsQuality = 0;
    
    //Audio Settings
    private float audioMaster = 1f;
    private float audioFX = 1f;
    private float audioMusic= 1f;

    private void Awake()
    {
        if (instance == null)
        {
            // Assign the global GameManager.instance object
            instance = this;
        }
        else
        {
            // Prevent duplicates in the scene
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        if (SceneManager.GetActiveScene().name.Equals("Main Menu"))
        {
            return;
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void SetQuality(int quality)
    {
        if (quality >= 0 && quality <= 2)
        {
            graphicsQuality = quality;
            QualitySettings.SetQualityLevel(quality, true);
        }
        else
        {
            Debug.LogWarning($"Quality index should be in range 0..2, index was {quality}", GameManager.instance);
        }
        
    }

    public void SetJoystickXSensitivity(float sensitivity)
    {
        joystickXSensitivity = sensitivity;
    }

    public void SetJoystickYSensitivity(float sensitivity)
    {
        joystickYSensitivity = sensitivity;
    }

    public void SetAudioMaster(float volumeMaster)
    {
        audioMaster = volumeMaster;
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volumeMaster) * 20);
    }

    public void SetAudioFX(float volumeFX)
    {
        audioFX = volumeFX;
        audioMixer.SetFloat("EffectsVol", Mathf.Log10(volumeFX) * 20);
    }

    public void SetAudioMusic(float volumeMusic)
    {
        audioMusic = volumeMusic;
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volumeMusic) * 20);
    }

    public float GetAudio(string type)
    {
        switch (type)
        {
            case "master":
                return audioMaster;
            case "fx":
                return audioFX;
            case "music":
                return audioMusic;
            default:
                Debug.LogWarning($"No known Audio type was returned: called Audio type was {type}", GameManager.instance);
                return 0f;
        }
    }

    void Update()
    {
        // DEBUG: this is only for testing the pausing feature.
        // Eventually this might need to be moved somewhere else
        if (Input.GetButtonDown("Pause"))
        {
            // Toggle pause
            if (IsPaused()) { UnPause(); }
            else { Pause(); }
        }
    }
}
