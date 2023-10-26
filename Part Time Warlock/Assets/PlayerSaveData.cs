using SaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerData myData = new PlayerData();
    private Player p;
    private UIManager uiManager;

    void Start()
    {
        p = GetComponent<Player>();
        uiManager = FindAnyObjectByType<UIManager>();
    }
    // Update is called once per frame
    void Update()
    {
        
        //constantly saving the player's position every frame
        //note: we might want to change this to every 30 seconds it saves or a checkpoint system, or something else
        myData.playerPosition = transform.position; 
        myData.playerRotation = transform.rotation;
        myData.coinCount = p.coinNum;

        //when the r key is pressed, all the data that was saved every frame is written to the file
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            /**
             * Note: potential security issues with JSON saving. Tech savvy players could just open it
             * in any text editor and give themselves the values they want
             */
            Debug.Log("Saving Game");
            SaveGameManager.currentSaveData.playerData = myData;
            SaveGameManager.SaveGame();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("LoadingGame");

            SaveGameManager.LoadGame();
            myData = SaveGameManager.currentSaveData.playerData;
            transform.position = myData.playerPosition;
            transform.rotation = myData.playerRotation;
            p.coinNum = myData.coinCount;
            uiManager.UpdateCoinText();
        }
    }
}

[System.Serializable]
public struct PlayerData
{
    //the data that you want to save from the player. Could be many different things
    //playerPosition is only an example
    public Vector2 playerPosition;
    public Quaternion playerRotation;
    public int coinCount;
}
