using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdiotSandwichLogic : MonoBehaviour
{
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = FindAnyObjectByType<Player>();
        player.controlsReversed = true;
    }
}