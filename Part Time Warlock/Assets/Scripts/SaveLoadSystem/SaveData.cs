using System;
using UnityEngine;

namespace SaveLoadSystem
{
    [System.Serializable]
    public class SaveData
    {
        /**
         * this is the data that needs to be saved. these are just some example
         * placeholders for some data that could be saved. This will be changed
         * for the actual game with the actual savedata
         */ 
        public PlayerData playerData = new PlayerData();
    }
}
