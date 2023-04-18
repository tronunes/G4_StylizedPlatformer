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

    public void SetJSSensitivity(System.Single sensitivity)
    {

    }

    public void SetMasterVol(System.Single volumeMaster)
    {
        gameManager.GetComponent<GameManager>().SetAudioMaster(volumeMaster);
    }

    public void SetFXVol(System.Single volumeFX)
    {
        gameManager.GetComponent<GameManager>().SetAudioFX(volumeFX);
    }

    public void SetMusicVol(System.Single volumeMusic)
    {
        gameManager.GetComponent<GameManager>().SetAudioMusic(volumeMusic);
    }

}
