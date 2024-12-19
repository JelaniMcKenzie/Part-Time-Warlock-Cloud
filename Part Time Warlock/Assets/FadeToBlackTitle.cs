using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackTitle : MonoBehaviour
{
    public RectTransform targetImage;   // The UI image to zoom in on
    public CanvasGroup overlayCanvasGroup; // The transparent overlay
    public float zoomSpeed = 2f;        // Speed of the zoom
    public float fadeSpeed = 1f;        // Speed of the fade
    public Transform zoomPoint;         // The point to zoom in on
    public float targetScale = 3f;      // Final scale value
    private bool isFadingAndZooming = false;
    [SerializeField] private loadSceneButton lsb;
    [SerializeField] private string sceneToOpen;
    [SerializeField] private GameObject[] titleScreenObjects;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // Trigger both effects with a key (Z here)
        {
            StartFadeAndZoom();
        }
    }

    public void StartFadeAndZoom()
    {
        if (!isFadingAndZooming)
        {
            isFadingAndZooming = true;
            for (int i = 0; i < titleScreenObjects.Length; i++)
            {
                FadeToTransparency(titleScreenObjects[i]);
            }
            StartCoroutine(FadeAndZoomCoroutine());
        }
    }

    private IEnumerator FadeAndZoomCoroutine()
    {
        float currentScale = targetImage.localScale.x;
        float overlayAlpha = overlayCanvasGroup.alpha;

        // Convert world position of zoomPoint to normalized pivot
        Vector3 localPoint = targetImage.InverseTransformPoint(zoomPoint.position);
        Vector2 normalizedPivot = new Vector2(
            Mathf.Clamp01((localPoint.x / targetImage.rect.width) + 0.5f),
            Mathf.Clamp01((localPoint.y / targetImage.rect.height) + 0.5f)
        );
        targetImage.pivot = normalizedPivot;

        while (currentScale < targetScale || overlayAlpha < 1f)
        {
            // Zoom in
            if (currentScale < targetScale)
            {
                currentScale += zoomSpeed * Time.deltaTime;
                targetImage.localScale = Vector3.one * currentScale;
            }

            // Fade overlay
            if (overlayAlpha < 1f)
            {
                overlayAlpha += fadeSpeed * Time.deltaTime;
                overlayCanvasGroup.alpha = Mathf.Clamp01(overlayAlpha);
            }

            yield return null;
        }

        isFadingAndZooming = false;
        lsb.LoadScene(sceneToOpen);
    }

    private void FadeToTransparency(GameObject target)
    {
        Image targetObjImage = target.GetComponent<Image>();
        StartCoroutine(FadeToTransparent(targetObjImage, fadeSpeed * 2));
    }

    private IEnumerator FadeToTransparent(Image targetImage, float _fadeSpeed)
    {
        Color originalColor = targetImage.color;

        // Fade until alpha reaches 0
        while (targetImage.color.a > 0f)
        {
            // Reduce the alpha based on fadeSpeed and Time.deltaTime
            float newAlpha = Mathf.Max(targetImage.color.a - _fadeSpeed * Time.deltaTime, 0f);

            targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        // Ensure alpha is set to 0 at the end
        targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
