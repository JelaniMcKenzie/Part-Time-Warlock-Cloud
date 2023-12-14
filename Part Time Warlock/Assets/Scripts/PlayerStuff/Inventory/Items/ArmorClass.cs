using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "(Armor)Armor", menuName = "InventoryPlus/Armor", order = 1)]
public class ArmorClass : InventoryPlus.Item
{
    
    public GameObject cloakBuffDebuff;
    public Color color = new Color();
    public Color defaultColor = new Color(170, 40, 137);

    public void EquipArmor(Player p, bool equipped)
    {
        if (equipped == true)
        {
            p.material.SetColor("_CloakSpaulders", color);
            
        }
        else
        {
            p.material.SetColor("_CloakSpaulders", defaultColor);
            
        }
        
    }

}
