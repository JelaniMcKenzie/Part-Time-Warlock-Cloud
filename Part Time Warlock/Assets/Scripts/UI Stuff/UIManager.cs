using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI timerText;
    public Image bag;
    public Image controls;
    public Image darkOverlay;
    public Sprite[] bagFill;
    public GameObject minimap;
    public GameObject miniMapHead;
    public WizardPlayer P;

    public Slider bossHealthBar;
    public bool isBossDead = false;

    public Scene activeScene;
    private GameManager gameManager;

    private Vector3 originalTextScale;
    private Vector3 originalTextPosition;

    //timer fields
    public float timer = 600f;
    public float minutes;
    public float seconds;
    public bool timerActive;
    public float timerSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        activeScene = SceneManager.GetActiveScene();
        timerActive = true;
        gameManager = FindAnyObjectByType<GameManager>();
        P = FindAnyObjectByType<WizardPlayer>();
        UpdateCoinText();

        if (activeScene.name == "Apartment")
        {
            //coinText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            timerActive = false;

            //bag.gameObject.SetActive(false);

            minimap.SetActive(false);
            miniMapHead.SetActive(false);
        }
        else if (activeScene.name == "RDG Test") {
            coinText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);

            bag.gameObject.SetActive(true);

            minimap.SetActive(true);
            miniMapHead.SetActive(true);
        }
        originalTextScale = timerText.transform.localScale;
        originalTextPosition = timerText.rectTransform.anchoredPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //hard coded solution to the canvas not being able to find the player
       
        
        //UpdatecoinText();

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
                gameManager.timeLeft += 1f;
                UpdateTimer(timer);
                //Debug.Log(timer);
            }

            else
            {
                timer = 0f;
                timerActive = false;
                //SceneManager.LoadScene("Lose");
            }
        }
        
        if (P.isGamePaused == false)
        {
            
            
        } 
        
        else
        {
            

        }

    }

    public void UpdateCoinText()
    {
        coinText.text = ": " + P.coinNum + " / 100";
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
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public IEnumerator ScaleTimerText()
    {
        float elapsedTime = 0f;
        float scaleFactor = 3f;
        float duration = 0.65f;
        // Calculate the target position for bottom-left growth
        Vector3 targetPosition = originalTextPosition + new Vector3(-timerText.rectTransform.rect.width * (scaleFactor - 1), -timerText.rectTransform.rect.height * (scaleFactor - 1), 0) / 2;

        // Scale up
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float scale = Mathf.Lerp(1f, scaleFactor, progress);
            timerText.transform.localScale = originalTextScale * scale;
            timerText.rectTransform.anchoredPosition = Vector3.Lerp(originalTextPosition, targetPosition, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // Scale down
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float scale = Mathf.Lerp(scaleFactor, 1f, progress);
            timerText.transform.localScale = originalTextScale * scale;
            timerText.rectTransform.anchoredPosition = Vector3.Lerp(targetPosition, originalTextPosition, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        timerText.transform.localScale = originalTextScale;
        timerText.rectTransform.anchoredPosition = originalTextPosition;
    }

    public void PauseGame()
    {
        pauseText.gameObject.SetActive(true);
        controls.gameObject.SetActive(true);
        darkOverlay.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseText.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
        darkOverlay.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
