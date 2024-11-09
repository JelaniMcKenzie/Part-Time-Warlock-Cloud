using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageVignette : MonoBehaviour
{
    public float intensity = 0f;

    Volume _volume;
    Vignette _vignette;

    private void Start()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _vignette);

        if (!_vignette)
        {
            Debug.LogError("error, vignette empty");
        }
        else
        {
            _vignette.active = false;
        }
    }

    public IEnumerator TakeDamageEffect()
    {
        intensity = 0.25f;

        _vignette.active = true;
        _vignette.intensity.Override(0.7f);

        while (intensity > 0f)
        {
            intensity -= 0.025f;

            if (intensity < 0f)
            {
                intensity = 0f;
            }

            _vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        _vignette.active = false;
        yield break;
    }
}
