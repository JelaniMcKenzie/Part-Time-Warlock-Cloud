using SaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameTester : MonoBehaviour
{
    public void SaveGame()
    {
        
        SaveGameManager.SaveGame();
    }

    public void LoadGame()
    {
        
        SaveGameManager.LoadGame();
        
    }
}
