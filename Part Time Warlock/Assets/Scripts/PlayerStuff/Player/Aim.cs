using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aim : MonoBehaviour
{
    [SerializeField] private Transform staffTip;
    public PlayerAnimations PAnim = null;
    public Scene activeScene;
    // Start is called before the first frame update
    void Start()
    {
        if (activeScene.name == "Apartment")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Cursor.visible = false;
        } 
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            Cursor.visible = true;
        }
        PAnim = FindAnyObjectByType<PlayerAnimations>();
    }

    // Update is called once per frame
    void Update()
    {

        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Set the crosshair position to the mouse position
        transform.position = mousePos;

        // Rotate the staff tip towards the crosshair position
        Vector3 staffDir = mousePos - staffTip.position;
        staffTip.up = staffDir;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
       
    }
}
