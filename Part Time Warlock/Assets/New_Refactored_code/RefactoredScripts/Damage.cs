using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private Player_Attributes Player = null;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player 1 (1)").GetComponent<Player_Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("hey something");

        if (other.CompareTag("Player"))
        {
            print("hey a player");
            Player_Attributes P = other.GetComponent<Player_Attributes>();

            if (P != null)
            {
                P.Damage();
            }
        }
    }
}
