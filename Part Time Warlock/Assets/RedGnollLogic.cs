using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGnollLogic : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        player.moveSpeed += 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
