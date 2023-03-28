using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    public Player P = null;
    
    
    // Start is called before the first frame update
    void Start()
    {
        P = FindObjectOfType<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Camera.main.transform.position = transform.position + new Vector3(0,0,-10);
        }
    }
}
