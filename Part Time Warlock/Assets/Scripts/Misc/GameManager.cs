using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string scene;
    public string endScene;
    public bool paused = false;
    public Player P = null;
    public UIManager UI = null;
    [SerializeField] public GameObject Portal = null;
    public float rentGoal = 25f;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
        UI = FindAnyObjectByType<UIManager>();
        Debug.Log(scene);
    }

    // Update is called once per frame
    void Update()
    {


        if (SceneManager.GetActiveScene().name == "Opener" || 
            SceneManager.GetActiveScene().name == "Opener2"||
            SceneManager.GetActiveScene().name == "Objectives" ||
            SceneManager.GetActiveScene().name == "Win" ||
            SceneManager.GetActiveScene().name == "Lose")
        {
            if (Input.anyKey && scene != null)
            {
                SceneManager.LoadScene(scene);
            }
            
        }
        
            
        /*else if (Input.anyKey)
            {
                SceneManager.LoadScene(endScene);
            }*/

        if (SceneManager.GetActiveScene().name == "RDG Test")
        {
            if (P.coinNum >= rentGoal)
            {
                Portal.SetActive(true);
            }
        }
        

        
        if (UI.timer == 0)
        {
            SceneManager.LoadScene("Lose");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
                P.canMove = false;
            }

            else
            {
                Time.timeScale = 1.0f;
                paused = false;
                P.canMove = true;
            }
        }
    }
}
