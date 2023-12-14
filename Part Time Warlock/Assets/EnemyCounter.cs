using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public Slime[] enemies;
    public GameObject portal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<Slime>();
        if (enemies.Length <= 0) {
            portal.SetActive(true);
        }
    }
}
