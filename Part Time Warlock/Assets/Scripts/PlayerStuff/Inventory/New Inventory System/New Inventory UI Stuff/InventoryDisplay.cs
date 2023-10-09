using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;
    protected NewInventorySystem inventorySystem;

    //Similar to a HashSet; pairs the slot in the UI to its slot in the backend
    //The UI slot is the "key" and the backend Slot is the "value" in the key-value pair
    protected Dictionary<InventorySlot_UI, SlotClass> slotDictionary; 

    public NewInventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, SlotClass> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(NewInventorySystem invToDisplay);

    protected virtual void UpdateSlot(SlotClass updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updatedSlot) //Slot value - the "under the hood" inventory slot.
            {
                slot.Key.UpdateUISlot(); //Slot key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedSlot)
    {
        Debug.Log("Slot Clicked");
    }
}
