using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderFuncs : MonoBehaviour
{
    [SerializeField] Slider sliderQuality;
    [SerializeField] Slider sliderAudioMaster;
    [SerializeField] Slider sliderAudioFX;
    [SerializeField] Slider sliderAudioMusic;
    [SerializeField] Slider sliderXsensitivity;
    [SerializeField] Slider sliderYsensitivity;

    void Awake() 
    {
        UpdateFromPrefs();
    }

    private void UpdateFromPrefs()
    {
        sliderQuality.value = (float)(PlayerPrefs.GetInt("GraphicsQuality", 0));
        SetQuality((float)PlayerPrefs.GetInt("GraphicsQuality", 0));

        sliderAudioMaster.value = PlayerPrefs.GetFloat("VolumeMaster", 1f);
        SetMasterVol(PlayerPrefs.GetFloat("VolumeMaster", 1f));

        sliderAudioFX.value = PlayerPrefs.GetFloat("VolumeEffects", 1f);
        SetFXVol(PlayerPrefs.GetFloat("VolumeEffects", 1f));

        sliderAudioMusic.value = PlayerPrefs.GetFloat("VolumeMusic", 1f);
        SetMusicVol(PlayerPrefs.GetFloat("VolumeMusic", 1f));

        sliderXsensitivity.value = PlayerPrefs.GetFloat("XSensitivity", 1f);
        SetXSensitivity(PlayerPrefs.GetFloat("XSensitivity", 1f));

        sliderYsensitivity.value = PlayerPrefs.GetFloat("YSensitivity", 1f);
        SetYSensitivity(PlayerPrefs.GetFloat("YSensitivity", 1f));
    }
    
    public void SetQuality(float qualityFloat)
    {
        int qualityIndex = Mathf.FloorToInt(qualityFloat);
        GameManager.instance.SetQuality(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
    }

    public void SetXSensitivity(float sensitivity)
    {
        GameManager.instance.SetJoystickXSensitivity(sensitivity);
        PlayerPrefs.SetFloat("XSensitivity", sensitivity);
    }

    public void SetYSensitivity(float sensitivity)
    {
        GameManager.instance.SetJoystickYSensitivity(sensitivity);
        PlayerPrefs.SetFloat("YSensitivity", sensitivity);
    }

    public void SetMasterVol(float volumeMaster)
    {
        GameManager.instance.SetAudioMaster(volumeMaster);
        PlayerPrefs.SetFloat("VolumeMaster", volumeMaster);
    }

    public void SetFXVol(float volumeFX)
    {
        GameManager.instance.SetAudioFX(volumeFX);
        PlayerPrefs.SetFloat("VolumeEffects", volumeFX);
    }

    public void SetMusicVol(float volumeMusic)
    {
        GameManager.instance.SetAudioMusic(volumeMusic);
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
    }

}
