using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class Laptop : MonoBehaviour
{
    public GameObject shopUI;
    public bool isActive;
    public WizardPlayer player;
    // Start is called before the first frame update
    private void Start()
    {
        player = FindAnyObjectByType<WizardPlayer>();
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                player.EnableMovement(false);
                shopUI.SetActive(true);
            }
        }
    }
}
