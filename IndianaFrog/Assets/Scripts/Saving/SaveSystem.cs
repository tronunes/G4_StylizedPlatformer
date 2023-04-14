using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(int completedLevel)
    {
        // Initialize binary formatter and file stream
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(GetFilePath(), FileMode.Create);

        // Save the data to a binary file
        SaveData saveData = new SaveData(completedLevel);
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
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
