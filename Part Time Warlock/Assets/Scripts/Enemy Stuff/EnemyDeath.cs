using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject CoinPrefab;
    public GameObject BigCoinPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int spawnCoin = Random.Range(0, 2);
        int spawnBigCoin = Random.Range(0, 6);
        if (spawnCoin == 1)
        {
            Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        }

        if (spawnBigCoin == 2)
        {
            Instantiate(BigCoinPrefab, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 2f);
    }
}
