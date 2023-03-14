using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Singleton, prevent duplicates
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

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
}
