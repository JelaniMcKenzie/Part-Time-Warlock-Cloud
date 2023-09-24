using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Staff_Attributes : MonoBehaviour
{
    public InventoryManager inventory;
    //public Transform staffArm;
    [SerializeField] public GameObject staffTip = null;
    public bool isInventoryOpen = false;
    public Player_Attributes p = null;
   // public GameObject crosshair;
    // Start is called before the first frame update
    void Start() 
    {
        //p = FindAnyObjectByType<Player_Attributes>();
    }

    void Update()
    {
           /* transform.localPosition = Vector2.MoveTowards(new Vector2(), crosshair.transform.position, 0);
            Vector2 pos = transform.localPosition;
            pos.x = Mathf.Min(0.1f, Mathf.Max(-0.1f, pos.x));
            pos.y = Mathf.Min(0.1f, Mathf.Max(-0.1f, pos.y));
            transform.localPosition = pos; */
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
    }


    // Update is called once per frame
    /* void Update()
     {
         if (isInventoryOpen == false)
         {
             //Use a spell or an item
             if (Input.GetMouseButtonDown(0))
             {

                 if (inventory.items[15].item.GetSpell() != null)
                 {
                     //use spell 1
                     inventory.items[15].item.GetSpell().Use(p);
                     //Debug.Log("Cast " + inventory.items[15].item.name);
                 }
             }
             else if (Input.GetMouseButtonDown(1))
             {
                 if (inventory.items[17].item.GetSpell() != null)
                 {
                     //use spell 2
                     inventory.items[17].item.GetSpell().Use(p);
                     //Debug.Log("Cast " + inventory.items[17].item.name);
                 }
             }
             else if (Input.GetKeyDown(KeyCode.Space))
             {
                 if (inventory.items[18].item.GetSpell() != null)
                 {
                     //use spell 3 (dash spell)
                     inventory.items[18].item.GetSpell().Use(p);
                     //Debug.Log("Cast " + inventory.items[18].item.name);
                 }
             }
             else if (Input.GetKeyDown(KeyCode.E))
             {
                 if (inventory.items[16].item.GetSpell() != null)
                 {
                     //use spell 4
                     inventory.items[16].item.GetSpell().Use(p);
                     //Debug.Log("Cast " + inventory.items[16].item.name);
                 }
             }
             else if (Input.GetKeyDown(KeyCode.Q))
             {
                 if (inventory.items[19].item != null)
                 {
                     //use item slot
                     inventory.items[19].item.Use(p);
                     //Debug.Log("Used " + inventory.items[19].GetItem().name);
                 }
             }
         }
     } */
}
