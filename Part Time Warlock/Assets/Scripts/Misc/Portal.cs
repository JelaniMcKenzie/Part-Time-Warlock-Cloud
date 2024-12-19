using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string scene;
    private WizardPlayer p;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        p = FindAnyObjectByType<WizardPlayer>();
        gm = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "RDG Test")
            {
                if (p.coinNum < gm.rentGoal)
                {
                    scene = "Lose";
                }
                Destroy(other.gameObject);
                
                
            }
            SceneManager.LoadScene(scene);
            this.gameObject.SetActive(false);


        }
    }
}
