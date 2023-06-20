using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject spellHolder;

    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private SlotClass[] startingItems;
    private SlotClass[] items;

    /*
    [SerializeField] private SpellClass spellToAdd;
    [SerializeField] private SpellClass spellToRemove;
    [SerializeField] private SlotClass[] startingSpells;
    private SlotClass[] spells;
    */

    private GameObject[] slots; //keeps the gameobject of every item slot
    //private GameObject[] spellSlots; //keeps the gameobject of every spell slot
    private GameObject basicShotSpell;
    private GameObject movementSpell;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        //spellSlots = new GameObject[spellHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        //spells = new SlotClass[spellSlots.Length];

        //initialize all the item & spell slots in the array with an instance of the SlotClass
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        
        /*for (int i = 0; i < spells.Length; i++)
        {
            spells[i] = new SlotClass();
        }*/


        //initialize any starter items & spells for the player
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }
        
        /*for (int i = 0; i < startingSpells.Length; i++)
        {
            spells[i] = startingSpells[i];
        }*/

        //set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        /*
        //set all the spell slots
        for (int i = 0; i < spellHolder.transform.childCount; i++)
        {
            spellSlots[i] = spellHolder.transform.GetChild(i).gameObject;
        }
        */

        RefreshUI();
        Add(itemToAdd, 1);
        Remove(itemToRemove);


        //RefreshSpellsUI();
        //AddSpell(spellToAdd);
        //RemoveSpell(spellToRemove);
        //ReplaceSpells();
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
        
        if (Input.GetMouseButtonDown(0)) //we left clicked
        {
            if (isMovingItem)
            {
                //end item move
                EndItemMove();
            } 
            else
            {
                //find the closest slot (the slot we clicked on)
                BeginItemMove();
            }
            
        }
        else if (Input.GetMouseButtonDown(1)) //we right clicked
        {
            if (isMovingItem)
            {
                //end item move
                EndItemMove_Single();
            }
            else
            {
                //find the closest slot (the slot we clicked on)
                BeginItemMove_Half();
            }
        }
    }

    #region Inventory Utils
    public void RefreshUI()
    {
        //look through each slots and spells gameobject
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                {
                    //display the quantity of items if its stackable
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
                }
                // gets the image in the respective item slot and sets the sprite equal to the item's icon
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                //if the inventory slot doesn't have an item, it'll display a blank text
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }

    /*
    public void RefreshSpellsUI()
    {
        for (int i = 0; i < spells.Length; i++)
        {
            try
            {
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = spells[i].GetItem().GetSpell().spellRune;   
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                // gets the image in the respective item slot and sets the sprite equal to the item's icon

            }
            catch
            {
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }
    */

    public bool Add(ItemClass item, int quantity)
    {
        //TODO: Fix bug where only one spell can be added to the list
        //items.Add(item);
        //check if inventory contains item
        SlotClass slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null) //if the item slot is empty
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
            /*
            //if there is enough space in the inventory, add it
            if (items.Count < slots.Length)
            {
                items.Add(new SlotClass(item, 1));
            }
            else
            {
                return false;
                //couldn't add item due to maxed out space
            }*/
            
        }
        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item)
    {
        //items.Remove(item);

        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubtractQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }

        }
        else
        {
            return false;
            //no need to change the UI because nothing changed about it
        }


        RefreshUI();
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() != null)
            {
                if (items[i].GetItem() == item)
                {
                    return items[i];
                }
            }
        }
        return null;
    }

    /*
    public bool AddSpell(SpellClass spell)
    {
        SlotClass spellSlot = ContainsSpell(spell);
        if (spellSlot == null)
        {
            for (int i = 0; i < spells.Length; i++)
            {
                if (spells[i].GetItem() == null)
                {
                    spells[i].AddItem(spell, 1);
                    Debug.Log("Spell Added");
                    break;
                }
            }
            //idea: do I need a separate spellmanager class?
            
        }
        else
        {
            Debug.Log("spellslots full");
            return false;
        }
        
        
        RefreshSpellsUI();
        return true;
    }
    
    public bool RemoveSpell(SpellClass spell)
    {
        SlotClass temp = ContainsSpell(spell);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubtractQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < spells.Length; i++)
                {
                    if (spells[i].GetItem() == spell)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                spells[slotToRemoveIndex].Clear();
            }

        }
        else
        {
            return false;
            //no need to change the UI because nothing changed about it
        }


        RefreshSpellsUI();
        return true;
    }
    
    public SlotClass ContainsSpell(SpellClass spell)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i].GetItem() != null)
            {
                if (spells[i].GetItem() == spell)
                {
                    return spells[i];
                }
            }
        }
        return null;
    }

    
    public void ReplaceSpells()
    {
        List<ProjectileSpellClass> moveBasicShot = new List<ProjectileSpellClass>();
        List<MovementSpellClass> moveDashSpell = new List<MovementSpellClass>();
        

        for (int i = 0; i < spellsList.Count; i++)
        {
            if (spellsList[i].GetSpell() is ProjectileSpellClass proj &&
                proj.isBasicShot == true)
            {
                moveBasicShot.Add(proj);
                spellsList.RemoveAt(i);
                i--;
            }

            if (spellsList[i].GetSpell() is MovementSpellClass dash)
            {
                moveDashSpell.Add(dash);
                spellsList.RemoveAt(i);
                i--;
            }  
        }

        int basicShotPosition = 0;
        int movementSpellPosition = 1;
        
        foreach(ProjectileSpellClass proj in moveBasicShot)
        {
            spellsList.Insert(basicShotPosition, new SpellSlotClass(proj));
        }

        foreach(MovementSpellClass dash in moveDashSpell)
        {
            spellsList.Insert(movementSpellPosition, new SpellSlotClass(dash));
        }

        RefreshSpellsUI();
    }
    */
    #endregion Inventory Utils

    #region Moving Stuff (click n drag)

    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false; //there is no item to move
        }

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        RefreshUI();
        isMovingItem = true;
        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false; //there is no item to move
        }

       
        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        originalSlot.SubtractQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));

        if (originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }
        RefreshUI();
        isMovingItem = true;
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear(); 
        }
        else
        {
            if (originalSlot.GetItem() != null) //does the slot already have an item
            {
                if (originalSlot.GetItem() == movingSlot.GetItem()) //they're the same item (they should stack)
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //swap the item with the one held by the cursor
                    tempSlot = new SlotClass(originalSlot); //a = b
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity()); //b = c
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity()); //a = c
                    RefreshUI();
                    return true;
                }
            }
            else //place item as usual
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }
        
        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            return false; //there is no item to move
        }
        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem())
        {
            return false;
        }

        movingSlot.SubtractQuantity(1);
        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            originalSlot.AddQuantity(1);
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
        }
        
        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true; 
        }
        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i];
            }
        }
        return null;
    }

    #endregion Moving Stuff (click n drag)


}
