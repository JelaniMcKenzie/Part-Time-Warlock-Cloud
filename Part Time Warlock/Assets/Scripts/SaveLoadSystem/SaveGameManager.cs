using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        var saveData = data;

        SaveLoad.Save(saveData);
    }

    public static void LoadData(SaveData _data)
    {
        data = _data;
    }

    public static void TryLoadData()
    {
        try
        {
            SaveLoad.Load();
        }
        catch
        {
            Debug.Log("Could not load save data");
        }
    }
}

