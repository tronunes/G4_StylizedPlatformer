using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Singleton, prevent duplicates
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private bool isPaused = false;

    //Input
    private float joystickXSensitivity = 1f;
    private float joystickYSensitivity = 1f;
    //Graphics Quality preset
    private int gcQuality = 0;
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
            gcQuality = quality;
            QualitySettings.SetQualityLevel(quality, true);
        }
        else
        {
            Debug.Log("Invalid quality index");
        }
        
    }

    public void SetJSXSensitivity(float sensitivity)
    {
        joystickXSensitivity = sensitivity;
        Debug.Log($"Joystick X sensitivity set to: {joystickXSensitivity}");
    }

    public void SetJSYSensitivity(float sensitivity)
    {
        joystickYSensitivity = sensitivity;
        Debug.Log($"Joystick Y sensitivity set to: {joystickYSensitivity}");
    }

    public void SetAudioMaster(float volumeMaster)
    {
        audioMaster = volumeMaster;
        Debug.Log($"Volume set to master: {audioMaster}, fx: {audioFX}, music: {audioMusic}");
    }

    public void SetAudioFX(float volumeFX)
    {
        audioFX = volumeFX;
        Debug.Log($"Volume set to master: {audioMaster}, fx: {audioFX}, music: {audioMusic}");
    }

    public void SetAudioMusic(float volumeMusic)
    {
        audioMusic = volumeMusic;
        Debug.Log($"Volume set to master: {audioMaster}, fx: {audioFX}, music: {audioMusic}");
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
