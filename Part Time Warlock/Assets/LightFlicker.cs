using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;
    private float originalIntensity;
    private bool isFlickering = false;

    public float flickerDuration = 1.0f;
    public float minFlickerIntensity = 0.3f;
    public float maxFlickerIntensity = 0.6f;
    public float flickerSpeed = 0.05f;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        if (light2D != null)
        {
            originalIntensity = light2D.intensity;
        }
        else
        {
            Debug.LogError("Light2D component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (!isFlickering) // Example trigger
        {
            StartCoroutine(FlickerLight());
        }
    }

    IEnumerator FlickerLight()
    {
        float randomNum = Random.Range(0, 15);
        isFlickering = true;
        float elapsed = 0f;

        while (elapsed < flickerDuration)
        {
            elapsed += flickerSpeed;
            light2D.intensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);
            yield return new WaitForSeconds(flickerSpeed);
        }

        light2D.intensity = originalIntensity;
        yield return new WaitForSeconds(randomNum);
        isFlickering = false;
        
    }

}
