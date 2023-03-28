using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public Player P;

    // Start is called before the first frame update
    void Start()
    {
        P = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(P.health / P.maxHealth, 0, 1f);
        Debug.Log("Health " + P.health);
    }
}
