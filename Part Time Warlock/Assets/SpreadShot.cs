using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.ProBuilder;

public class SpreadShot : MonoBehaviour
{
    GameObject[] children;
    WizardPlayer p;
    public float speed;
    private void Start()
    {
        p = FindAnyObjectByType<WizardPlayer>();
        children = new GameObject[this.transform.childCount];
        // Calculate the direction from the player's position to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - p.staffTip.transform.position; // Remove normalization

        for (int i = 0; i < children.Length; i++)
        {
            direction += new Vector2(direction.x, (direction.y + children[i].transform.rotation.z));
            children[i] = this.transform.GetChild(i).gameObject;
            Rigidbody2D rb2d = children[i].GetComponent<Rigidbody2D>();
            rb2d.velocity = direction.normalized * speed;
            // Calculate the rotation angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Rotate the projectile to face the mouse position
            children[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
