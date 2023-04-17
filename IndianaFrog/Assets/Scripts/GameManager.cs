using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Singleton, prevent duplicates
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private bool isPaused = false;

    //Graphics Quality preset
    private int gcQuality = 2;
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

    public void SetAudio(float master = 1f, float fx = 1f, float music = 1f)
    {
        audioMaster = master;
        audioFX = fx;
        audioMusic = music;
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
