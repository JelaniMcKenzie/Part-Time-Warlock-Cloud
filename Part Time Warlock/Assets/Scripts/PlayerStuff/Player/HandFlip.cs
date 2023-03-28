using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFlip : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("HAND");
           

        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("HAND");
            
            
        }
    }

    public void HandFlipLeft()
    {
        transform.localPosition = new Vector3(-0.5f, transform.localPosition.y, 0f);
        //transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    public void HandFlipRight()
    {
        transform.localPosition = new Vector3(0.5f, transform.localPosition.y, 0f);
        //transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

}
