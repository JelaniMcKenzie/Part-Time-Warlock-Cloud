using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemController : MonoBehaviour
{
    public PassiveItemClass passiveItem;
    public GameObject passiveAbilityRef;
    public bool isActiveInScene;

    // Reference to the equipped armor data

    public Player player;

    private void Start()
    {
        player = this.gameObject.GetComponent<Player>();

    }

    public GameObject instantiatedAbilityRef;

    private void Update()
    {
        /*foreach (InventoryPlus.ItemSlot item in player.inventory.inventoryItems)
        {
            if (item.GetItemType() != null && item.GetItemType().itemCategory == "Passive")
            {
                ActivatePassiveItem((PassiveItemClass)item.GetItemType());
                
            }
            else
            {
                continue;
                /*passiveAbilityRef = null;
                if (isActiveInScene == true)
                {
                    isActiveInScene = false;
                }

                // Check if abilityRef is instantiated and then destroy it
                if (instantiatedAbilityRef != null)
                {
                    Destroy(instantiatedAbilityRef);
                    instantiatedAbilityRef = null; // Reset the reference
                }
            }
        }*/

    }

}
