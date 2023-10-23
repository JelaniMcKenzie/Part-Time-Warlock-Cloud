using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject shopUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                shopUI.SetActive(true);
            }
        }
    }
}
