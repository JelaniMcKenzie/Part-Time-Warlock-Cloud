using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image manaBarImage;
    public Player P = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateManaBar() 
    {
        manaBarImage.fillAmount = Mathf.Clamp(P.mana / P.maxMana, 0, 1f);
        Debug.Log("Mana " + P.mana);
    }
}
