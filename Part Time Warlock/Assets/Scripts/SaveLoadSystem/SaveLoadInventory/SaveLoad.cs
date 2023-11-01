using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public static class SaveLoad 
{
    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> OnLoadGame;

    private static string directory = "/SaveData/";
    private static string fileName = "SaveGame.sav";

    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        //Application.presistentDataPath is the LocalLow folder on PC's C: drive by default
        string dir = Application.persistentDataPath + directory;

        GUIUtility.systemCopyBuffer = dir;

        if (!Directory.Exists(dir)) //if the file path doesn't exist, make it
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(data, true); //convert the data to a string to print to a file;
        File.WriteAllText(dir + fileName, json);
        Debug.Log("Saving Game");
        return true;
    }

    public static SaveData Load() 
    { 
        //try to load .sav file from directory
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();
        if (File.Exists(fullPath)) 
        { 
            //get file and turn it back into a json string
            var json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);
            OnLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("Save File does not exist");
        }

        return data;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
