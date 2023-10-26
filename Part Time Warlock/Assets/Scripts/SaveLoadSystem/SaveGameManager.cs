using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace SaveLoadSystem
{
    public static class SaveGameManager
    {
        public static SaveData currentSaveData = new SaveData();

        public const string saveDirectory = "/SaveData/";
        public const string fileName = "SaveGame.sav";

        public static UnityAction OnLoadGameStart;
        public static UnityAction OnLoadGameFinish;

        public static bool SaveGame()
        {
            var dir = Application.persistentDataPath + saveDirectory;

            if (!Directory.Exists(dir)) 
            {
                Directory.CreateDirectory(dir);
            }

            string json = JsonUtility.ToJson(currentSaveData, true);
            File.WriteAllText(dir + fileName, json);

            GUIUtility.systemCopyBuffer = dir;

            return true;
        }

        public static void LoadGame()
        {
            OnLoadGameStart?.Invoke(); //in all the scripts that need it, subscribe to this event with a method(s)
                                       //to preform logic when a game starts loading
            string fullPath = Application.persistentDataPath + saveDirectory + fileName;
            SaveData tempData = new SaveData();

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                tempData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogError("Save file does not exist!");
            }

            currentSaveData = tempData;
            OnLoadGameFinish?.Invoke();//in all the scripts that need it, subscribe to this event with a method(s)
                                       //to preform logic when a game is done loading

        }
    }
}
