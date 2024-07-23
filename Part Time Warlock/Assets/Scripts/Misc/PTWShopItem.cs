using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTWShopItem : MonoBehaviour
{
    public GameObject buyMessage;

    public ShopManager shopManager;
    
    public UIManager uiManager;

    private bool inBuyZone;

    public bool isPassiveItem, isSpell, isCloak;

    public InventoryPlus.Item item;

    public WizardPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<WizardPlayer>();
        shopManager = FindAnyObjectByType<ShopManager>();
        uiManager = FindAnyObjectByType<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (player.coinNum >= item.price)
                {
                    player.coinNum -= item.price;
                    player.inventory.AddInventory(item, 1, 0, true);
                    //shopManager.RemoveItemFromList(item);
                    uiManager.UpdateCoinText();
                    this.gameObject.SetActive(false);
  
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        inBuyZone = true;
        if (collision.CompareTag("Player"))
        {
            buyMessage.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        inBuyZone = false;
        if (collision.CompareTag("Player"))
        {
            buyMessage.SetActive(false);
        }
    }
}
