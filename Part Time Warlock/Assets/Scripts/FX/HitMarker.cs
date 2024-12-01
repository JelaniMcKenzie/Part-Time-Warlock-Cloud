using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{

    public ParticleSystem ps = null;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, ps.main.duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
