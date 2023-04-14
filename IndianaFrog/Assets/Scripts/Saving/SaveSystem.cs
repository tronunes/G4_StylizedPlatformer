using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(int completedLevel)
    {
        // Check if an existing save file is OK to overwrite
        SaveData currentSaveData = LoadGame();
        if (
            currentSaveData != null &&
            completedLevel > currentSaveData._levelCompleted
        )
        {
            // Initialize binary formatter and file stream
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(GetFilePath(), FileMode.Create);

            // Save the data to a binary file
            SaveData newSaveData = new SaveData(completedLevel);
            binaryFormatter.Serialize(fileStream, newSaveData);
            fileStream.Close();
        }

    }

    public static SaveData LoadGame()
    {
        SaveData loadData = null;

        // Case: save found
        if (File.Exists(GetFilePath()))
        {
            // Initialize binary formatter and file stream
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(GetFilePath(), FileMode.Open);

            // Load the data from a binary file
            loadData = binaryFormatter.Deserialize(fileStream) as SaveData;
            fileStream.Close();
        }
        // Case: save NOT found
        else
        {
            Debug.LogWarning("Save file not found in " + GetFilePath());
        }

        return loadData;
    }

    private static string GetFilePath()
    {
        return Application.persistentDataPath + "/save.bin";
    }
}
