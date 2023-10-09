using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class SpellLibrary : MonoBehaviour
{
    public SlotClass[] spellsLibrary;

    public void TransferSpells(InventoryManager im)
    {
        for (int i = 0; i < spellsLibrary.Length; i++)
        {
            if (im.GetClosestSlot() == spellsLibrary[i])
            {

            }
        }
    }
}
