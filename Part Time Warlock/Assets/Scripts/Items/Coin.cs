using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public UIManager UI = null;
    public Player P = null;
    [SerializeField] public AudioClip CoinSound = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindObjectOfType<Player>();
        UI = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(CoinSound, transform.position, 4);
            if (tag == "Coin")
            {
                P.coinNum++;
                UI.UpdateCoinText();
                Destroy(this.gameObject);
            }
            else if (tag == "BigCoin")
            {
                AudioSource.PlayClipAtPoint(CoinSound, transform.position, 4);
                P.coinNum += 5;
                UI.UpdateCoinText();
                Destroy(this.gameObject);
            }
        }
    }
}
