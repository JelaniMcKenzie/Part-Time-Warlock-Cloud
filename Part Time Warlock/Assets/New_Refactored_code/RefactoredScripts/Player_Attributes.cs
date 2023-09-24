using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attributes : MonoBehaviour
{
    [Header("Player Variables")]
    public float speed;
    public int maxHealth;
    public int health;


    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveInput;

    [SerializeField] public GameObject staffTip = null;


    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {





        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        rb.velocity = moveInput * speed;
    }

    public enum MovementState
    {
        walking,
        sprinting
    }
    private void StateHandler()
    {

    } 




    public void Damage()
    {
        health--;
        //Instantiate(DamagePrefab, transform.position, Quaternion.identity);

        if (health < 1)
        {
            //Instantiate(DeathPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
