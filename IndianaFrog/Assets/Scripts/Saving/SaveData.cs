using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int _levelCompleted;

    public SaveData(int completedLevel)
    {
        _levelCompleted = completedLevel;
    }
}
