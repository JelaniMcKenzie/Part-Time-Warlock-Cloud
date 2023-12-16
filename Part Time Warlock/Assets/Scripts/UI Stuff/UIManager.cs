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
    public Image bag;
    public Image controls;
    public Image darkOverlay;
    public Sprite[] bagFill;
    public GameObject minimap;
    public GameObject miniMapHead;
    public Player P;

    public Scene activeScene;
    private GameManager gameManager;

    //timer fields
    public float timer = 300f;
    public float minutes;
    public float seconds;
    public bool timerActive;
    public float timerSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        gameManager = FindAnyObjectByType<GameManager>();
        //UpdateCoinText();
        
        if (activeScene.name == "Apartment")
        {
            CoinText.gameObject.SetActive(false);
            TimerText.gameObject.SetActive(false);
            RentGoalText.gameObject.SetActive(false);

            bag.gameObject.SetActive(false);

            minimap.SetActive(false);
            miniMapHead.SetActive(false);
        }
        else if (activeScene.name == "RDG Test") {
            CoinText.gameObject.SetActive(true);
            TimerText.gameObject.SetActive(true);
            RentGoalText.gameObject.SetActive(true);

            bag.gameObject.SetActive(true);

            minimap.SetActive(true);
            miniMapHead.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //hard coded solution to the canvas not being able to find the player
        //change later so that this code only runs after the dungeon is finished generating
        P = FindAnyObjectByType<Player>();
        
        //UpdateCoinText();

        if (bag != null)
        {
            UpdateBagImage(P.coinNum, gameManager.rentGoal);
        }
        
        if (P == null)
        {
            Debug.Log("couldn't find player");
        }
        
        if (timerActive == true)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * timerSpeed;
                UpdateTimer(timer);
                //Debug.Log(timer);
            }

            else
            {
                timer = 0f;
                timerActive = false;
                SceneManager.LoadScene("Lose");
            }
        }
        
        if (P.isGamePaused == false)
        {
            Pausetext.gameObject.SetActive(false);
            controls.gameObject.SetActive(false);
            darkOverlay.gameObject.SetActive(false);
        } 
        
        else
        {
            Pausetext.gameObject.SetActive(true);
            controls.gameObject.SetActive(true);
            darkOverlay.gameObject.SetActive(true);

        }

    }

    public void UpdateCoinText()
    {
        CoinText.text = ": " + P.coinNum;
    }

    // Call this method to update the image based on the percentage
    public void UpdateBagImage(float numerator, float denominator)
    {
        if (bag != null && bagFill != null && bagFill.Length > 0)
        {
            float percentage = numerator / denominator;

            // Calculate the index based on fifths
            int index = Mathf.FloorToInt(percentage * bagFill.Length);

            // Ensure the index is within the valid range
            index = Mathf.Clamp(index, 0, bagFill.Length - 1);

            // Assign the corresponding sprite
            bag.sprite = bagFill[index];
        }
        else
        {
            Debug.LogError("Image component or fifths sprites are not assigned.");
        }
    }

    public void UpdateTimer(float timeToDisplay)
    {
        timeToDisplay += 1;
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
