using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public Player P = null;
    public UIManager UI = null;
    public HealthBar HB = null;
    public AudioClip HPotion = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindObjectOfType<Player>();
        UI = FindObjectOfType<UIManager>();
        HB = FindObjectOfType<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            AudioSource.PlayClipAtPoint(HPotion, transform.position, 4);
            P.health += 0.2f;
            HB.UpdateHealthBar();
            Destroy(this.gameObject);
        }
    }
}
