using UnityEngine;
using UnityEngine.SceneManagement;


namespace InventoryPlus
{
    public class DontDestroy : MonoBehaviour
    {
        
        private void Awake()
        {
            UnityEngine.SceneManagement.Scene activeScene = SceneManager.GetActiveScene();
            DontDestroy[] playerParents = FindObjectsOfType<DontDestroy>();
            
            if (activeScene.name != "Apartment" || activeScene.name != "RDG Test") {
                //destroy duplicates if they exist, keep this
                if (playerParents.Length != 1) GameObject.Destroy(playerParents[1].gameObject);
                else GameObject.DontDestroyOnLoad(this);
            }
            
        }
    }
}