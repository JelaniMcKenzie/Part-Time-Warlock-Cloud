using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public WizardPlayer P = null;
    public UIManager UI = null;
    public HealthBar HB = null;
    public AudioClip HPotion = null;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<WizardPlayer>();
        UI = FindAnyObjectByType<UIManager>();
        HB = FindAnyObjectByType<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            AudioSource.PlayClipAtPoint(HPotion, transform.position, 4);
            
            if (HB != null)
            {
                //HB.UpdateHealthBar();
            }
           
            Destroy(this.gameObject);
        }
    }
}
