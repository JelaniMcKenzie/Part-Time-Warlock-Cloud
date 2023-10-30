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
    public float dropOffset = 1f; //how far the item is dropped from the player

    private Transform playerTransform;
    private void Awake()
    {
        itemSprite.color = Color.clear;
        itemCount.text = "";
        itemSprite.preserveAspect = true;

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerTransform == null )
        {
            Debug.Log("player not found");
        }
    }

    private void Update()
    {
        //TODO: add controller support.
        if (AssignedMouseInvSlot.Item != null)
        {
            transform.position = Mouse.current.position.ReadValue(); //have the selected item follow the mouse

            //if left click outside of inventory, delete the item
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject()) 
            {
                if (AssignedMouseInvSlot.Item.itemPrefab != null)
                {
                    Instantiate(AssignedMouseInvSlot.Item.itemPrefab, playerTransform.position + (playerTransform.up * -1) * dropOffset, Quaternion.identity);
                    
                }
                if (AssignedMouseInvSlot.Quantity > 1)
                {
                    AssignedMouseInvSlot.SubtractQuantity(1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();
                }
                

                //TODO : Drop the item on the ground instead of deleting it 
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
        UpdateMouseSlot();
    }

    public void UpdateMouseSlot()
    {
        itemSprite.sprite = AssignedMouseInvSlot.Item.itemIcon;
        itemCount.text = AssignedMouseInvSlot.Quantity.ToString();
        itemSprite.color = Color.white;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0; //if the list of hit UI elements is greater than 0, that means the mouse interacted with something.
                                  //if not, then it hit nothing
    }
}
