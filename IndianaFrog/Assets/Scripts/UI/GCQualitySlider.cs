using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCQualitySlider : MonoBehaviour
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

}
