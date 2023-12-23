using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace InventoryPlus
{
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]
    public class InputReader : MonoBehaviour
    {
        public enum ActionState { Inventory, HUD, Both };

        [Header("Inputs")]
        public string InventoryOnHorizontalInput = "Horizontal";
        public string InventoryOffHorizontalInput = "Mouse ScrollWheel";
        public bool enableMouseInventoryOff = false;

        [Space(15)]
        [Header("Actions")]
        public ActionState performUse = ActionState.Both;
        public ActionState performDrop = ActionState.Both;
        public ActionState performSort = ActionState.Inventory;
        public ActionState performSwap = ActionState.Inventory;
        public ActionState performEquip = ActionState.Inventory;

        [Space(15)]
        [Header("Audio")]
        public bool playAudioOnSelection = false;
        public AudioSource selectionAudio;

        [Space(15)]
        [Header("References")]
        public Player player;
        public Inventory inventory;
        public UIDetails details;


        public bool inventoryOn = false;

        private GameObject currentSelectedObj = null;
        private StandaloneInputModule inputModule;
        private EventSystem eventSystem;


        /**/


        #region Setup

        private void Awake()
        {
            inputModule = this.GetComponent<StandaloneInputModule>();
            eventSystem = this.GetComponent<EventSystem>();

            //set initial state
            inputModule.horizontalAxis = InventoryOffHorizontalInput;
            inventory.SelectFirstHotbarSlot();
            inventory.ClearSwap();
        }

        #endregion


        #region Inputs

        private void Update()
        {
            UpdateSelection();

            ToggleInventory();
            InventoryActions();
        }


        public void UpdateSelection()
        {
            GameObject tmpObj = currentSelectedObj;

            //handle selection when no GameObject are selected
            if (eventSystem.currentSelectedGameObject != null) currentSelectedObj = eventSystem.currentSelectedGameObject;
            else eventSystem.SetSelectedGameObject(currentSelectedObj);

            //handle selection change
            if (tmpObj != currentSelectedObj)
            {
                if (playAudioOnSelection) selectionAudio.Play();
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), true);
            }
        }


        private void ToggleInventory()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryOn = !inventoryOn;

                ShowCursor(inventoryOn);
                player.EnableMovement(!inventoryOn);
                inventory.ShowInventory(inventoryOn);
                inventory.ForceEndSwap();

                //inventory open - inventory closed
                if (inventoryOn)
                {
                    inputModule.horizontalAxis = InventoryOnHorizontalInput;

                    //Select the last selected object upon opening the inventory
                    //e.g., if a spell was selected before the inventory was closed, that spell's info
                    //would be shown upon opening the inventory
                    currentSelectedObj.GetComponent<Button>().Select();
                    currentSelectedObj.GetComponent<Button>().OnSelect(null);
                    details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), true);

                    //if the last selected object doesn't have an item, autoselect the first spell slot
                    if (inventory.GetInventorySlot(currentSelectedObj.GetComponent<UISlot>()) == null)
                    {
                        inventory.SelectFirstHotbarSlot();
                    }

                }
                else
                {
                    inputModule.horizontalAxis = InventoryOffHorizontalInput;
                    currentSelectedObj.GetComponent<Button>().OnDeselect(null);

                }
            }
        }


        private void InventoryActions()
        {
            /*
            //use action
            if (Input.GetKeyDown(KeyCode.U) && currentSelectedObj != null && ((performUse == ActionState.Inventory && inventoryOn) || (performUse == ActionState.HUD && !inventoryOn) || (performUse == ActionState.Both)))
            {
                inventory.UseItem(currentSelectedObj.GetComponent<UISlot>());
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), false);
            }*/

            /*
            //drop action
            if (Input.GetKeyDown(KeyCode.Q) && currentSelectedObj != null && ((performDrop == ActionState.Inventory && inventoryOn) || (performDrop == ActionState.HUD && !inventoryOn) || (performDrop == ActionState.Both)))
            {
                inventory.DropItem(currentSelectedObj.GetComponent<UISlot>());
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), false);
            }*/

            //sort action
            if (Input.GetKeyDown(KeyCode.M) && currentSelectedObj != null && ((performSort == ActionState.Inventory && inventoryOn) || (performSort == ActionState.HUD && !inventoryOn) || (performSort == ActionState.Both)))
            {
                inventory.Sort();
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), false);
            }

            /*
            //equip action
            if (Input.GetKeyDown(KeyCode.E) && currentSelectedObj != null && ((performEquip == ActionState.Inventory && inventoryOn) || (performEquip == ActionState.HUD && !inventoryOn) || (performEquip == ActionState.Both)))
            {
                inventory.EquipItem(currentSelectedObj.GetComponent<UISlot>());
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), false);
            }*/

            //swap action - clear swap
            if (Input.GetKeyDown(KeyCode.N) && currentSelectedObj != null && ((performSwap == ActionState.Inventory && inventoryOn) || (performSwap == ActionState.HUD && !inventoryOn) || (performSwap == ActionState.Both)))
            {
                inventory.SwapItem(currentSelectedObj.GetComponent<UISlot>());
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inventory.ClearSwap();
                if (details != null && inventoryOn) details.UpdateDetails(currentSelectedObj.GetComponent<UISlot>(), false);
            }
        }

        #endregion


        #region Utils

        private void ShowCursor(bool _show)
        {
            if (!enableMouseInventoryOff)
            {
                if (_show)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    /*
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    */
                }
            }
        }

        #endregion
    }
}