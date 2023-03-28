using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] Tomes;
    [SerializeField] public Image TomeDisplay = null;
    public Text CoinText;
    public Text TomeText;
    public Text Pausetext = null;
    public Text TimerText = null;
    public Player P = null;

    //timer fields
    public float timer = 300f;
    public float minutes;
    public float seconds;
    public bool timerActive;

    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        P = FindObjectOfType<Player>();
        //TomeDisplay.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void UpdateCoinText()
    {
        CoinText.text = ": " + P.coinNum;
    }

    public void UpdateTome(int numTome)
    {
        TomeDisplay.sprite = Tomes[numTome];
    }

    public void UpdateTimer(float timeToDisplay)
    {
        timeToDisplay += 1;
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
