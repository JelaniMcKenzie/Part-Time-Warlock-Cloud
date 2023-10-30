using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text itemCount;
    [SerializeField] private SlotClass assignedInventorySlot; //backend slot represented by UI element

    //Note: Button stuff may be commented out based on needs of the game
    private Button button;

    public SlotClass AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    public void Awake()
    {
        ClearSlot();

        itemSprite.preserveAspect = true;

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnUISlotClick); //this requires an EventSystem gameObject in your scene to work
        }
        else
        {
            Debug.Log("Cannot Find Button");
        }
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void InitializeSlot(SlotClass slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(SlotClass slot) //pass in a backend inventory slot to display
    {
        if (slot.Item != null)
        {
            itemSprite.sprite = slot.Item.itemIcon;
            itemSprite.color = Color.white;

            if (slot.Quantity > 1) itemCount.text = slot.Quantity.ToString();
            else itemCount.text = "";
        }
        else
        {
            ClearSlot();
        }
       
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
        {
            UpdateUISlot(assignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.Clear();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        if (ParentDisplay != null)
        {
            ParentDisplay.SlotClicked(this);            
        }
    }
}
