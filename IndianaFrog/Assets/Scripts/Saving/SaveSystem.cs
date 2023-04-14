using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SaveGame(int completedLevel)
    {
        // Check if an existing save file is OK to overwrite
        SaveData previousSaveData = LoadGame();
        if (
            previousSaveData == null ||
            completedLevel > previousSaveData._levelCompleted
        )
        {
            // Serialize the save data to JSON format
            SaveData newSaveData = new SaveData(completedLevel);
            string jsonData = JsonUtility.ToJson(newSaveData, true);

            // Initialize the file stream and stream writer
            Directory.CreateDirectory(Path.GetDirectoryName(GetFilePath()));
            FileStream fileStream = new FileStream(GetFilePath(), FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            // Save the data to a JSON file
            streamWriter.Write(jsonData);

            // Close file stream and writer
            streamWriter.Close();
            fileStream.Close();
        }
    }

    public static SaveData LoadGame()
    {
        SaveData loadData = null;

        // Case: save found
        if (File.Exists(GetFilePath()))
        {
            // Initialize the file stream and stream reader
            FileStream fileStream = new FileStream(GetFilePath(), FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);

            // Load the data from a JSON file
            string jsonData = streamReader.ReadToEnd();

            // Close file stream and reader
            fileStream.Close();
            streamReader.Close();

            // Deserialize the JSON data
            loadData = JsonUtility.FromJson<SaveData>(jsonData);
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
        return Application.persistentDataPath + "/save.json";
    }
}
