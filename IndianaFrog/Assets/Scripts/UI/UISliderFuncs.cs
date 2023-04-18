using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderFuncs : MonoBehaviour
{
    //public GameObject gameManager;
    void Awake() 
    {
        //gameManager = GameObject.Find("GameManager");
    }
    
    public void SetQuality(System.Single qualityIndex)
    {
        switch (qualityIndex)
        {
            case 0:
                GameManager.instance.SetQuality(0);
                Debug.Log("Quality set to: " + QualitySettings.names[0]);
                break;
            case 1:
                GameManager.instance.SetQuality(1);
                Debug.Log("Quality set to: " + QualitySettings.names[1]);
                break;
            case 2:
                GameManager.instance.SetQuality(2);
                Debug.Log("Quality set to: " + QualitySettings.names[2]);
                break;
        }
    }

    public void SetJSSensitivity(System.Single sensitivity)
    {

    }

    public void SetMasterVol(System.Single volumeMaster)
    {
        GameManager.instance.SetAudioMaster(volumeMaster);
    }

    public void SetFXVol(System.Single volumeFX)
    {
        GameManager.instance.SetAudioFX(volumeFX);
    }

    public void SetMusicVol(System.Single volumeMusic)
    {
        GameManager.instance.SetAudioMusic(volumeMusic);
    }

}
