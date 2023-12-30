using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    public AudioClip firewallclip = null;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(firewallclip, transform.position, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        Destroy(this.gameObject, 5f);
    }
}
