/**
     * A quick note on syntax; this code makes use of UnityActions
     * as defined in the parent class. UnityActions are custom events
     * that can be managed within scripts. 
     * 
     * Subscribing to an event means
     * adding a method (that we wrote) to the list of methods that can be
     * called when an event is raised. In the code below, the event is
     * OnDynamicInventoryDisplayRequested, and the method that is added
     * is RefreshDynamicInventory, which we also wrote below. Subscribing
     * to an event is done with the += operator.
     * 
     * Unsubscribing to an event does the exact opposite; it removes a method
     * from the list of methods that can be called when an event is raised. In 
     * the same example, OnDynamicInventoryDisplayRequested is the event, and
     * RefreshDynamicInventory is the removed method. Unsubscribing to an event
     * is done with the -= operator.
     */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false); //close the dynamic inventory by default
        playerBackpackPanel.gameObject.SetActive(false); //close the player inventory by default

    }
    private void OnEnable()
    {
        NewInventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;

    }

    private void OnDisable()
    {
        NewInventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;

    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            inventoryPanel.gameObject.SetActive(false); //close the inventory panel;
        }

        if (playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            playerBackpackPanel.gameObject.SetActive(false); //close the inventory panel;
        }
    }

    void DisplayInventory(NewInventorySystem invToDisplay, int offset)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}
