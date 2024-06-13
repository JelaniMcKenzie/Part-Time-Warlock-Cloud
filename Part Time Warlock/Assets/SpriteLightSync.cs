using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpriteLightSync : MonoBehaviour
{
    public Light2D lightRef;
    public SpriteRenderer parentSprite;
    // Start is called before the first frame update
    void Start()
    {
        lightRef = GetComponent<Light2D>();
        parentSprite = GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lightRef.lightCookieSprite = parentSprite.sprite;
    }
}
