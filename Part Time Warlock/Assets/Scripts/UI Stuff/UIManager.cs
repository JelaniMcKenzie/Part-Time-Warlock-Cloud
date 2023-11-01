using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI Pausetext;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI RentGoalText;
    public Image[] GoldCoins;
    public Player P;

    public Scene activeScene;

    //timer fields
    public float timer = 300f;
    public float minutes;
    public float seconds;
    public bool timerActive;

    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay;

    private void Awake()
    {
        shopKeeperDisplay.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
    }

    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
    }

    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        shopKeeperDisplay.gameObject.SetActive(true);
        shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
    }

    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        P = FindAnyObjectByType<Player>();
        UpdateCoinText();
        
        if (activeScene.name != "Apartment")
        {
            CoinText.gameObject.SetActive(false);
            TimerText.gameObject.SetActive(false);
            RentGoalText.gameObject.SetActive(false);

            foreach (Image coin in GoldCoins)
            {
                coin.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCoinText();
        if (timerActive == true)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                UpdateTimer(timer);
                //Debug.Log(timer);
            }

            else
            {
                timer = 0f;
                timerActive = false;
            }
        }
        
        if (P.canMove == false)
        {
            Pausetext.gameObject.SetActive(true);
        } 
        
        else
        {
            Pausetext.gameObject.SetActive(false);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            shopKeeperDisplay.gameObject.SetActive(false);
        }
    }

    public void UpdateCoinText()
    {
        CoinText.text = ": " + P.coinNum;
    }

    public void UpdateTimer(float timeToDisplay)
    {
        timeToDisplay += 1;
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
