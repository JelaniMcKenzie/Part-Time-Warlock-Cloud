using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseItemData : MonoBehaviour
{
    public Image itemSprite;
    public Text itemCount;
    public SlotClass AssignedMouseInvSlot;

    private void Awake()
    {
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    private void Update()
    {
        if (AssignedMouseInvSlot.Item != null)
        {
            transform.position = Mouse.current.position.ReadValue(); //have the selected item follow the mouse

            //if left click outside of inventory, delete the item
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject()) 
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        AssignedMouseInvSlot.Clear();
        itemCount.text = "";
        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
    }

    public void UpdateMouseSlot(SlotClass invSlot)
    {
        AssignedMouseInvSlot.AssignItem(invSlot); //give the mouse an actual backend item to hold
        itemSprite.sprite = invSlot.Item.itemIcon;
        itemCount.text = invSlot.Quantity.ToString();
        itemSprite.color = Color.white;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;   
    }
}
