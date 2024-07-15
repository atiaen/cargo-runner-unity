using System;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/gamedata.json";

    public static void SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(saveFilePath, json);
            //Debug.Log("Game saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save game: " + e.Message);
        }
    }

    public static SaveData LoadGame()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                return data;
            }
            else
            {
                return null; // Return default data if no save file found
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null; // Return default data on error
        }
    }
}
