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
    public InventoryDisplay parentDisplay { get; private set; }

    public void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);
        parentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void InitializeSlot(SlotClass slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(SlotClass slot)
    {
        if (slot.item != null)
        {
            itemSprite.sprite = slot.item.itemIcon;
            itemSprite.color = Color.white;

            if (slot.quantity > 1) itemCount.text = slot.quantity.ToString();
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
        itemCount.text = string.Empty;
    }

    public void OnUISlotClick()
    {
        parentDisplay?.SlotClicked(this);
    }
}
