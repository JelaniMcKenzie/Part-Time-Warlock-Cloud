using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] public int PowerUpID ;
    private Player P = null;
    private UIManager UI = null;
    public ManaBar MB = null;
    [SerializeField] public AudioClip PowerUpSound = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
        UI = FindAnyObjectByType<UIManager>();
        MB = FindAnyObjectByType<ManaBar>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(PowerUpSound, transform.position, 4);
            if (PowerUpID == 0)
            {
                if (P.canFire == true)
                {
                    P.canFire = false;
                    P.spellFire = 0;
                }
                P.mana = P.maxMana;
                MB.UpdateManaBar();
                P.currentTome = 1;
                P.canIce = true;
                P.spellIce = 5;
                UI.UpdateTome(PowerUpID);
                UI.TomeDisplay.enabled = true;
            }

            else if(PowerUpID == 1)
            {
                if (P.canIce == true)
                {
                    P.canIce = false;
                    P.spellIce = 0;
                }
                P.mana = P.maxMana;
                MB.UpdateManaBar();
                P.currentTome = 2;
                P.canFire = true;
                P.spellFire = 3;
                UI.UpdateTome(PowerUpID);
                UI.TomeDisplay.enabled = true;
            }

            else if (PowerUpID == 2)
            {
                if (P.canFire == true || P.canIce == false)
                {
                    P.canFire = false;
                    P.spellFire = 0;
                    P.canIce = false;
                    P.spellIce = 0;                   
                }
                P.mana = P.maxMana;
                MB.UpdateManaBar();
                P.currentTome = 3;
                P.canLightning = true;
                P.spellLightning = 2;
                UI.UpdateTome(PowerUpID);
                UI.TomeDisplay.enabled = true;
            }
            Destroy(this.gameObject);
        }
    }
}
