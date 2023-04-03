using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderFuncs : MonoBehaviour
{
    public void SetQuality(System.Single qualityIndex)
    {
        switch (qualityIndex)
        {
            case 0:
                QualitySettings.SetQualityLevel(0, true);
                Debug.Log("Quality set to: " + QualitySettings.names[0]);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1, true);
                Debug.Log("Quality set to: " + QualitySettings.names[1]);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2, true);
                Debug.Log("Quality set to: " + QualitySettings.names[2]);
                break;
        }
    }

    public void SetMasterVol(System.Single value)
    {
        AudioListener.volume = value;
        Debug.Log("Master Volume set to: " + value);
    }

    public void SetFXVol(System.Single value)
    {
        //needs to change some global multiplier
        
    }

    public void SetMusicVol(System.Single value)
    {
        //needs to change some global multiplier 
        
    }

}
