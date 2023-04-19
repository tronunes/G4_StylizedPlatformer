using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderFuncs : MonoBehaviour
{
    void Awake() 
    {
        
    }
    
    public void SetQuality(System.Single qualityIndex)
    {
        switch (qualityIndex)
        {
            case 0f:
                GameManager.instance.SetQuality(0);
                Debug.Log("Quality set to: " + QualitySettings.names[0]);
                break;
            case 1f:
                GameManager.instance.SetQuality(1);
                Debug.Log("Quality set to: " + QualitySettings.names[1]);
                break;
            case 2f:
                GameManager.instance.SetQuality(2);
                Debug.Log("Quality set to: " + QualitySettings.names[2]);
                break;
        }
    }

    public void SetXSensitivity(System.Single sensitivity)
    {
        GameManager.instance.SetJSXSensitivity(sensitivity);
    }

    public void SetYSensitivity(System.Single sensitivity)
    {
        GameManager.instance.SetJSYSensitivity(sensitivity);
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
