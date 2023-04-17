using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderFuncs : MonoBehaviour
{
    GameObject gameManager;
    void Awake() 
    {
        gameManager = GameObject.Find("GameManager");
    }
    
    public void SetQuality(System.Single qualityIndex)
    {
        switch (qualityIndex)
        {
            case 0:
                gameManager.GetComponent<GameManager>().SetQuality(0);
                Debug.Log("Quality set to: " + QualitySettings.names[0]);
                break;
            case 1:
                gameManager.GetComponent<GameManager>().SetQuality(1);
                Debug.Log("Quality set to: " + QualitySettings.names[1]);
                break;
            case 2:
                gameManager.GetComponent<GameManager>().SetQuality(2);
                Debug.Log("Quality set to: " + QualitySettings.names[2]);
                break;
        }
    }

    public void SetMasterVol(System.Single value)
    {
        AudioListener.volume = value;
        gameManager.GetComponent<GameManager>().SetAudio(master: value);
        Debug.Log("Master Volume set to: " + value);
    }

    public void SetFXVol(System.Single value)
    {
        gameManager.GetComponent<GameManager>().SetAudio(fx: value);
    }

    public void SetMusicVol(System.Single value)
    {
        gameManager.GetComponent<GameManager>().SetAudio(music: value);
    }

}
