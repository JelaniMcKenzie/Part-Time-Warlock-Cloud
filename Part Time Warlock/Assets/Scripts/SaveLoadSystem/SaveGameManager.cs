using System.IO;
using UnityEngine;


namespace SaveLoadSystem
{
    public static class SaveGameManager
    {
        public static SaveData currentSaveData = new SaveData();

        public const string saveDirectory = "/SaveData/";
        public const string fileName = "SaveGame.sav";
        public static bool Save()
        {
            var dir = Application.persistentDataPath + saveDirectory;

            if (!Directory.Exists(dir)) {
              Directory.CreateDirectory(dir);
            }
            return true;
        }
    }
}
