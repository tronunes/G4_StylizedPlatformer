using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Singleton, prevent duplicates
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private bool isPaused = false;

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
