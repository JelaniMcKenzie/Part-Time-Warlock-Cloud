using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public UIManager UI = null;
    public WizardPlayer player = null;
    [SerializeField] public AudioClip CoinSound = null;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<WizardPlayer>();
        UI = FindAnyObjectByType<UIManager>();
        if (UI == null)
        {
            Debug.Log("cant find UI");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(CoinSound, transform.position, 4);
            if (CompareTag("Coin"))
            {
                player.coinNum++;
                UI.UpdateCoinText();
                Destroy(this.gameObject);
            }
            else if (CompareTag("BigCoin"))
            {
                //AudioSource.PlayClipAtPoint(CoinSound, transform.position, 4);
                player.coinNum += 5;
                UI.UpdateCoinText();
                Destroy(this.gameObject);
            }
        }
    }
}
