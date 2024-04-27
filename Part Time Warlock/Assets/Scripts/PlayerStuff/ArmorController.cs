using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    public ArmorClass equippedArmor;
    public GameObject abilityRef;

    public bool isEquipped = false;

    // Reference to the equipped armor data

    public Player player;

    private void Start()
    {
        player = this.gameObject.GetComponent<Player>();
        
    }

    private GameObject instantiatedAbilityRef;

    private void Update()
    {
            // If the armor slot contains an item, swap the palette
            if (player.inventory.GetInventorySlot(player.inventory.hotbarUISlots[5]) != null)
            {
                ArmorClass a = (ArmorClass)player.inventory.GetInventorySlot(player.inventory.hotbarUISlots[5]).GetItemType();
                equippedArmor = a;
                abilityRef = equippedArmor.cloakBuffDebuff;
                isEquipped = true;
                equippedArmor.EquipArmor(player, isEquipped);

                // Check if abilityRef is not already instantiated
                if (instantiatedAbilityRef == null)
                {
                    instantiatedAbilityRef = Instantiate(abilityRef);
                }
            }
            else
            {
                abilityRef = null;
                if (isEquipped == true) 
                {
                    isEquipped = false;
                    equippedArmor.EquipArmor(player, isEquipped);
                }
                
                
                // Check if abilityRef is instantiated and then destroy it
                if (instantiatedAbilityRef != null)
                {
                    Destroy(instantiatedAbilityRef);
                    instantiatedAbilityRef = null; // Reset the reference
                }
            }
        
        
    }


}
