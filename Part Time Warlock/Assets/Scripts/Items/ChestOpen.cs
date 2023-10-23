using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField] public GameObject CoinPrefab = null;
    [SerializeField] public GameObject BigCoinPrefab = null;
    [SerializeField] public GameObject ClosedChest = null;

    public bool closed;

    public float threshold = 15f;
    public float cnt = 30f;

    // Start is called before the first frame update
    void Start()
    {
        closed = false;
        int spawnTome = Random.Range(0, 2);
        int spawnBigCoin = Random.Range(0, 3);


        if (spawnBigCoin == 0)
        {
            Instantiate(BigCoinPrefab, transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
        }

        else if (spawnBigCoin == 1 || spawnBigCoin == 2)
        {
            Instantiate(CoinPrefab, transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (closed == false)
        {
            StartCoroutine(CloseChest());
        }
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public IEnumerator CloseChest()
    {
        closed = false;
        yield return new WaitForSeconds(30f);
        closed = true;
        if (closed == true)
        {
            Instantiate(ClosedChest, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        
    }
}
