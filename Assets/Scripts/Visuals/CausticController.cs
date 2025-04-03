using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalYPositionController : MonoBehaviour
{
    public float targetYValue = 553f;  // The Y value where the decal should remain
    public float fadeDuration = 2f;    // Duration for the fade effect

    private DecalProjector decalProjector;
    private float fadeTimer = 0f;
    private bool isFading = false;
    private bool isVisible = false;

    private void Awake()
    {
        // Get the Decal Projector component
        decalProjector = GetComponent<DecalProjector>();

        // Disable the decal in the editor
        if (!Application.isPlaying)
        {
            decalProjector.enabled = false;
            isVisible = false;
        }
    }

    private void Start()
    {
        // Enable the decal when the game starts
        decalProjector.enabled = true;

        // Set the decal's position to always have the target Y value
        SetDecalYPosition();

        // Start with the decal fully visible or invisible based on the object's initial Y position
        SetDecalAlpha(transform.position.y <= targetYValue ? 1f : 0f);
    }

    private void Update()
    {
        // Keep the decal at the specified Y position
        SetDecalYPosition();

        // Check if the decal is above the target Y value
        if (transform.position.y > targetYValue && isVisible)
        {
            SetDecalAlpha(0f);
            isVisible = false;
        }
        else if (transform.position.y <= targetYValue && !isVisible)
        {
            isFading = true;
            fadeTimer = 0f;
            isVisible = true;
        }

        // Handle fade-in effect
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float fadeValue = Mathf.Clamp01(fadeTimer / fadeDuration);
            SetDecalAlpha(fadeValue);

            if (fadeValue >= 1f)
            {
                isFading = false;
            }
        }
    }

    private void OnDestroy()
    {
            SetDecalAlpha(0f);
            isVisible = false;
    }

    private void SetDecalYPosition()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, targetYValue, currentPosition.z);
    }

    private void SetDecalAlpha(float alpha)
    {
        if (decalProjector.material.HasProperty("_Alpha"))
        {
            decalProjector.material.SetFloat("_Alpha", alpha);
        }
    }
}
