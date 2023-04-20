using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderFuncs : MonoBehaviour
{
    [SerializeField] Slider sliderQuality;
    GameObject sliderAudioMaster;
    GameObject sliderAudioFX;
    GameObject sliderAudioMusic;
    GameObject sliderXsensitivity;
    GameObject sliderYsensitivity;
    void Awake() 
    {
        BindSliderVariables();
        UpdateFromPrefs();
    }

    private void BindSliderVariables()
    {
        //sliderQuality = GameObject.Find("TMPSlider_Quality/TMPSlider_Quality").GetComponent<Slider>();
        sliderAudioMaster = GameObject.Find("TMPSlider_Master");
        sliderAudioFX = GameObject.Find("TMPSlider_Effects");
        sliderAudioMusic = GameObject.Find("TMPSlider_Music");
        sliderXsensitivity = GameObject.Find("Slider_XSensitivity");
        sliderYsensitivity = GameObject.Find("Slider_YSensitivity");
    }

    private void UpdateFromPrefs()
    {
        Debug.Log("Print From PlayerPrefs: " + PlayerPrefs.GetFloat("GCQuality", 0f));
        sliderQuality.value = PlayerPrefs.GetFloat("GCQuality", 0f);
        
        SetQuality(PlayerPrefs.GetFloat("GCQuality", 0f));
    }
    
    public void SetQuality(System.Single qualityIndex)
    {
        switch (qualityIndex)
        {
            case 0f:
                GameManager.instance.SetQuality(0);
                PlayerPrefs.SetFloat("GCQuality", 0f);
                Debug.Log("Quality set to: " + QualitySettings.names[0]);
                break;
            case 1f:
                GameManager.instance.SetQuality(1);
                PlayerPrefs.SetFloat("GCQuality", 1f);
                Debug.Log("Quality set to: " + QualitySettings.names[1]);
                break;
            case 2f:
                GameManager.instance.SetQuality(2);
                PlayerPrefs.SetFloat("GCQuality", 2f);
                Debug.Log("Quality set to: " + QualitySettings.names[2]);
                break;
        }
    }

    public void SetXSensitivity(System.Single sensitivity)
    {
        GameManager.instance.SetJSXSensitivity(sensitivity);
        PlayerPrefs.SetFloat("XSensitivity", sensitivity);
    }

    public void SetYSensitivity(System.Single sensitivity)
    {
        GameManager.instance.SetJSYSensitivity(sensitivity);
        PlayerPrefs.SetFloat("YSensitivity", sensitivity);
    }

    public void SetMasterVol(System.Single volumeMaster)
    {
        GameManager.instance.SetAudioMaster(volumeMaster);
        PlayerPrefs.SetFloat("VolumeMaster", volumeMaster);
    }

    public void SetFXVol(System.Single volumeFX)
    {
        GameManager.instance.SetAudioFX(volumeFX);
        PlayerPrefs.SetFloat("VolumeEffects", volumeFX);
    }

    public void SetMusicVol(System.Single volumeMusic)
    {
        GameManager.instance.SetAudioMusic(volumeMusic);
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
    }

}
