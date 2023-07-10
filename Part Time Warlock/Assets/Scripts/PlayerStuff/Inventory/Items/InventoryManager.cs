using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    //A list of every available crafting recipe
    //Idea: should this list be for unlocked recipes only?
    //Should I have a separate list for unlocked recipes?
    [SerializeField] private List<CraftingRecipeClass> craftingRecipes = new List<CraftingRecipeClass>();
    [SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject spellSlotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [SerializeField] private SlotClass[] startingItems = new SlotClass[15];
    [SerializeField] private SlotClass[] loadout = new SlotClass[5]; 

    public SlotClass slotTypeRef = new SlotClass();

    public SlotClass[] items;

    private GameObject[] slots; //keeps the gameobject of every item slot
    private GameObject[] spellSlots; //keeps the gameobject of every spell slot

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;
    public Player P = null;

    #region deprecated spell code
    /*
    [SerializeField] private SpellClass spellToAdd;
    [SerializeField] private SpellClass spellToRemove;
    [SerializeField] private SlotClass[] startingSpells;
    
    */



    //private GameObject basicShotSpell;
    //private GameObject movementSpell;
    #endregion deprecated spell code

    

    private void Start()
    {
        P = FindAnyObjectByType<Player>();
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        spellSlots = new GameObject[spellSlotHolder.transform.childCount];

        for (int i = 0; i < spellSlots.Length; i++)
        {
            spellSlots[i] = spellSlotHolder.transform.GetChild(i).gameObject;
        }

        //initialize all the item slots in the array with an instance of the SlotClass
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();

            if (i >= 15 && i < items.Length - 1)//Set the four elements before the last element of the array to a spell slot type
            {
                items[i].slotType = SlotClass.SlotType.spell;
            }
            if (i == 19) //set the last element of the inventory array to a potion slot type
            {
                items[i].slotType = SlotClass.SlotType.potion;
            }
        }

         //set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        //initialize any starter items & spells for the player
        for (int i = 0; i < startingItems.Length; i++) //15 is a hard coded limit that is proportional to the # of slots in the inventory
        {
            Add(startingItems[i].item, startingItems[i].quantity);
        }

        //set the four spells to be equal to the 16th, 17th, 18th, and 19th items in the inventory
        //note: these values are hardcoded and will likely need to change later
        for (int i = 0; i < loadout.Length - 1; i++)
        {
   
            if (loadout[i].item is not SpellClass)
            {
                loadout[i].Clear();
            }
            if (loadout[4].item is not ConsumableClass)
            {
                loadout[4].Clear();
            }
            items[15] = loadout[0];
            items[16] = loadout[1];
            items[17] = loadout[2];
            items[18] = loadout[3];
            items[19] = loadout[4];
        }
        
        
        

       

        RefreshUI();

        /*for (int i = 0; i < startingSpells.Length; i++)
        {
            spells[i] = startingSpells[i];
        }
        //set all the spell slots
        for (int i = 0; i < spellSlotHolder.transform.childCount; i++)
        {
            spellSlots[i] = spellSlotHolder.transform.GetChild(i).gameObject;
        }
        
        //RefreshSpellsUI();
        //AddSpell(spellToAdd);
        //RemoveSpell(spellToRemove);
        //ReplaceSpells();*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //handle crafting
        {
            Craft(craftingRecipes[0]); //the only crafting recipe currently loaded
            //TODO: Create a system that passes the correct recipe into the Craft() method
            //based on a user's input of some kind
        }

        
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            if (movingSlot.item is SpellClass)
            {
                itemCursor.GetComponent<Image>().sprite = movingSlot.item.GetSpell().spellRune;
            }
            else
            {
                itemCursor.GetComponent<Image>().sprite = movingSlot.item.itemIcon;
            }
        }
        if (P.isInventoryOpen == true)
        {
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
        
    }

    #region Inventory Utils
    public void RefreshUI()
    {
        //look through each slots and spells gameobject
        for (int i = 15; i < slots.Length; i++)
        {
            slots[i].transform.GetChild(0).GetComponent<Image>().rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            slots[i].transform.GetChild(1).GetComponent<Text>().transform.rotation = Quaternion.Euler(0, 0, 0);
            
        }

        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                if (items[i].item is SpellClass)
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].item.GetSpell().spellRune;

                }
                else
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].item.itemIcon;
                }


                if (items[i].item.isStackable)
                {
                    //display the quantity of items if its stackable
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].quantity + "";
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

        

        RefreshSpellsUI();
    }



    
    public void RefreshSpellsUI()
    {
        for (int i = 0; i < spellSlots.Length; i++)
        {
            try
            {
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                if (items[i + (spellSlots.Length * 3)].item is SpellClass)
                {
                    spellSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (spellSlots.Length * 3)].item.GetSpell().spellRune;
                }
                else
                {
                    spellSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (spellSlots.Length * 3)].item.itemIcon;

                }
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                if (items[i + (spellSlots.Length * 3)].item.isStackable)
                {
                    //display the quantity of items if its stackable
                    spellSlots[i].transform.GetChild(1).GetComponent<Text>().text = items[i + (spellSlots.Length * 3)].quantity + "";
                    spellSlots[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rotation = Quaternion.Euler(0, 0, 0);

                }
                else
                {
                    spellSlots[i].transform.GetChild(1).GetComponent<Text>().rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                    spellSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
                }
                // gets the image in the respective item slot and sets the sprite equal to the item's icon
            }
            catch
            {
                Debug.Log("Failed to load Spell");
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                spellSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                //if the inventory slot doesn't have an item, it'll display a blank text
                spellSlots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }


    public bool Add(ItemClass item, int quantity)
    {
        SlotClass slot = Contains(item);
        if (slot != null)
        {
            int quantityCanAdd = slot.item.stackSize - slot.quantity;
            int quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);

            int remainder = quantity - quantityCanAdd;

            slot.AddQuantity(quantityToAdd);
            if (remainder > 0) Add(item, remainder);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].item == null)
                {
                    int quantityCanAdd = item.stackSize - items[i].quantity;
                    int quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);

                    int remainder = quantity - quantityCanAdd;

                    items[i].AddItem(item, quantityToAdd);
                    if (remainder > 0) Add(item, remainder);

                    break;
                }
            }
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
            if (temp.quantity > 1)
            {
                temp.SubtractQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].item == item)
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

    //Remove method overload specifically for crafting
    public bool Remove(ItemClass item, int quantity)
    {
        //items.Remove(item);

        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.quantity > 1)
            {
                temp.SubtractQuantity(quantity);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].item == item)
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

    public void UseConsumable()
    {
        items[19].SubtractQuantity(1);
        RefreshUI();
    }

    public bool isInventoryFull()
    {
        for (int i = 0; i < items.Length - 5; i++)
        {
            if (items[i].item == null) //do we have an empty item slot
            {
                return false;
            }
        }
        return true; //the inventory is full
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item && items[i].item.isStackable && items[i].quantity < items[i].item.stackSize)
            {
                return items[i];
            }
        }

        return null;
    }

    //Maybe change the name/functionality of this method to specifically deal with crafting
    public bool Contains(ItemClass item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == item && items[i].quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    /*
    public bool AddSpell(SpellClass spell)
    {
        SlotClass spellSlot = ContainsSpell(spell);
        if (spellSlot == null)
        {
            for (int i = 0; i < spells.Length; i++)
            {
                if (spells[i].item == null)
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
            if (temp.quantity > 1)
            {
                temp.SubtractQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < spells.Length; i++)
                {
                    if (spells[i].item == spell)
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
            if (spells[i].item != null)
            {
                if (spells[i].item == spell)
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
        if (originalSlot == null || originalSlot.item == null)
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
        if (originalSlot == null || originalSlot.item == null)
        {
            return false; //there is no item to move
        }

       
        movingSlot = new SlotClass(originalSlot.item, Mathf.CeilToInt(originalSlot.quantity / 2f));
        originalSlot.SubtractQuantity(Mathf.CeilToInt(originalSlot.quantity / 2f));

        if (originalSlot.quantity == 0)
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
        

        switch (originalSlot.slotType)  
        {
            //check if the item can actually move into that slot
            case (SlotClass.SlotType.spell):
                {
                    if (movingSlot.item is not SpellClass) 
                    {
                        Debug.Log("Can't move there");
                        return false; 
                    }
                    if (originalSlot == null)
                    {

                        Add(movingSlot.item, movingSlot.quantity);
                        movingSlot.Clear();
                    }
                    else
                    {
                        if (originalSlot.item != null) //does the slot already have an item
                        {
                            if (originalSlot.item == movingSlot.item && originalSlot.item.isStackable
                                && originalSlot.quantity < originalSlot.item.stackSize) //they're the same item (they should stack)
                            {
                                var quantityCanAdd = originalSlot.item.stackSize - originalSlot.quantity;
                                var quantityToAdd = Mathf.Clamp(movingSlot.quantity, 0, quantityCanAdd);
                                var remainder = movingSlot.quantity - quantityToAdd;


                                Debug.Log(remainder);
                                originalSlot.AddQuantity(quantityToAdd);
                                if (remainder <= 0)
                                {
                                    movingSlot.Clear();
                                }
                                else
                                {
                                    movingSlot.SubtractQuantity(quantityCanAdd);
                                    RefreshUI();
                                    return false;
                                }

                            }
                            else
                            {
                                //swap the item with the one held by the cursor
                                tempSlot = new SlotClass(originalSlot); //a = b
                                originalSlot.AddItem(movingSlot.item, movingSlot.quantity); //b = c
                                movingSlot.AddItem(tempSlot.item, tempSlot.quantity); //a = c
                                RefreshUI();
                                return true;
                            }
                        }
                        else //place item as usual
                        {
                            originalSlot.AddItem(movingSlot.item, movingSlot.quantity);
                            movingSlot.Clear();
                        }
                    }
                    break;
                }
            case (SlotClass.SlotType.potion):
                {
                    if (movingSlot.item is not ConsumableClass) 
                    {
                        Debug.Log("Can't Move There");
                        return false; 
                    }
                    if (originalSlot == null)
                    {

                        Add(movingSlot.item, movingSlot.quantity);
                        movingSlot.Clear();
                    }
                    else
                    {
                        if (originalSlot.item != null) //does the slot already have an item
                        {
                            if (originalSlot.item == movingSlot.item && originalSlot.item.isStackable
                                && originalSlot.quantity < originalSlot.item.stackSize) //they're the same item (they should stack)
                            {
                                var quantityCanAdd = originalSlot.item.stackSize - originalSlot.quantity;
                                var quantityToAdd = Mathf.Clamp(movingSlot.quantity, 0, quantityCanAdd);
                                var remainder = movingSlot.quantity - quantityToAdd;


                                Debug.Log(remainder);
                                originalSlot.AddQuantity(quantityToAdd);
                                if (remainder <= 0)
                                {
                                    movingSlot.Clear();
                                }
                                else
                                {
                                    movingSlot.SubtractQuantity(quantityCanAdd);
                                    RefreshUI();
                                    return false;
                                }

                            }
                            else
                            {
                                //swap the item with the one held by the cursor
                                tempSlot = new SlotClass(originalSlot); //a = b
                                originalSlot.AddItem(movingSlot.item, movingSlot.quantity); //b = c
                                movingSlot.AddItem(tempSlot.item, tempSlot.quantity); //a = c
                                RefreshUI();
                                return true;
                            }
                        }
                        else //place item as usual
                        {
                            originalSlot.AddItem(movingSlot.item, movingSlot.quantity);
                            movingSlot.Clear();
                        }
                    }
                    break;
                }
            default:
                {
                    if (originalSlot == null)
                    {

                        Add(movingSlot.item, movingSlot.quantity);
                        movingSlot.Clear();
                    }
                    else
                    {
                        if (originalSlot.item != null) //does the slot already have an item
                        {
                            if (originalSlot.item == movingSlot.item && originalSlot.item.isStackable 
                                && originalSlot.quantity < originalSlot.item.stackSize) //they're the same item (they should stack)
                            {
                                var quantityCanAdd = originalSlot.item.stackSize - originalSlot.quantity;
                                var quantityToAdd = Mathf.Clamp(movingSlot.quantity, 0, quantityCanAdd);
                                var remainder = movingSlot.quantity - quantityToAdd;


                                Debug.Log(remainder);               
                                originalSlot.AddQuantity(quantityToAdd);
                                if (remainder <= 0)
                                {
                                    movingSlot.Clear();
                                }
                                else
                                {
                                    movingSlot.SubtractQuantity(quantityCanAdd);
                                    RefreshUI();
                                    return false;
                                }
                                    
                            }
                            else
                            {
                                //swap the item with the one held by the cursor
                                tempSlot = new SlotClass(originalSlot); //a = b
                                originalSlot.AddItem(movingSlot.item, movingSlot.quantity); //b = c
                                movingSlot.AddItem(tempSlot.item, tempSlot.quantity); //a = c
                                RefreshUI();
                                return true;
                            }
                        }
                        else //place item as usual
                        {
                            originalSlot.AddItem(movingSlot.item, movingSlot.quantity);
                            movingSlot.Clear();
                        }
                    }
                    break;
                }
        }
       
        
        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot is null)
            return false;
        if (originalSlot.item is not null &&
            (originalSlot.item != movingSlot.item || originalSlot.quantity >= originalSlot.item.stackSize))
            return false;

        //movingSlot.SubQuantity(1);
        if (originalSlot.item != null && originalSlot.item == movingSlot.item)
            originalSlot.AddQuantity(1);
        else
            originalSlot.AddItem(movingSlot.item, 1);
        movingSlot.SubtractQuantity(1);
        if (movingSlot.quantity < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
            isMovingItem = true;

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

    private void Craft(CraftingRecipeClass recipe)
    {
        if (recipe.CanCraft(this)) //if we can craft the item using ingredients in the player's inventory
        {
            recipe.Craft(this); //Craft the item and put it in the player's inventory
        }
        else
        {
            //show error message saying you can't craft this item
            Debug.Log("Can't craft that item");
        }
    }
}
