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
        /*Debug.Log(transform.localPosition);
        GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        dir = Vector3.MoveTowards(new Vector3(), dir, 0.1f) * 1000;
        float xDistance = Mathf.Abs(dir.x);
        float yDistance = Mathf.Abs(dir.y);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 50;
        GetComponent<RectTransform>().position = pos;*/

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

        /*if (transform.localPosition.x < -1)
        {
            HF.HandFlipLeft();
        }*/

        /*if (transform.localPosition.x > 1)
        {
            HF.HandFlipRight();
        }*/
        
       
    }
}
