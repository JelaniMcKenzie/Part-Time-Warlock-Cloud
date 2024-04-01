using UnityEngine.SceneManagement;
using UnityEngine;


namespace InventoryPlus
{
    public class ChangeScene : MonoBehaviour
    {
        [Header("Refernces")]
        [Min(0)] public int sceneIndex;
        public string playerTag = "Player";
        public InventorySaver inventorySaver;
        public GameObject inventoryObj;
        public Player P;
        
        
        private SaveSystem saveSystem;


        /**/


        private void Awake()
        {
            saveSystem = this.GetComponent<SaveSystem>();    
            inventorySaver = FindAnyObjectByType<InventorySaver>();
            P = FindAnyObjectByType<Player>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag))
            {
                //inventorySaver.UpdateSavedInventory(inventoryObj.GetComponent<Inventory>());
                if (saveSystem != null) saveSystem.SaveData();
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }
}