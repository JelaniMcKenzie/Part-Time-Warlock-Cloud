using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] public GameObject CoinPrefab = null;
    [SerializeField] public GameObject BigCoinPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        int spawnCoin = Random.Range(0, 2);
        int spawnBigCoin = Random.Range(0, 6);
        if (spawnCoin == 1)
        {
            GameObject K = Instantiate(CoinPrefab, transform.position, Quaternion.identity);
            K.transform.parent = null;
        }

        if (spawnBigCoin == 2)
        {
            Instantiate(BigCoinPrefab, transform.position, Quaternion.identity);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject,
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
