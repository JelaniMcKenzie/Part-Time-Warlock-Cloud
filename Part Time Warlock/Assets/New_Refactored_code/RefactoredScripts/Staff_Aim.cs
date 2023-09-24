using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Staff_Aim : MonoBehaviour
{
    
    // Start is called before the first frame update

    //public Vector2 PointerPosition { get; set; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.right = (PointerPosition - (Vector2)transform.position).normalized;

        

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        transform.rotation = rotation;
    }
}
