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

    private void Start()
    {
        // Get the Decal Projector component from the GameObject
        decalProjector = GetComponent<DecalProjector>();

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
            // Instantly hide the decal
            SetDecalAlpha(0f);
            isVisible = false;
        }
        // If the decal is below or at the Y value and not yet visible, fade it in
        else if (transform.position.y <= targetYValue && !isVisible)
        {
            // Start fading in
            isFading = true;
            fadeTimer = 0f;
            isVisible = true;
        }

        // Handle fade-in effect
        if (isFading)
        {
            fadeTimer += Time.deltaTime;

            // Fade in based on time progression
            float fadeValue = Mathf.Clamp01(fadeTimer / fadeDuration);
            SetDecalAlpha(fadeValue);

            // Stop fading when fully visible
            if (fadeValue >= 1f)
            {
                isFading = false;
            }
        }
    }

    // Function to ensure the decal's Y position stays at the target value
    private void SetDecalYPosition()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, targetYValue, currentPosition.z);
    }

    // Helper function to set the alpha of the decal's material
    private void SetDecalAlpha(float alpha)
    {
        // Assuming the shader has an "_Alpha" property for transparency control
        decalProjector.material.SetFloat("_Alpha", alpha);
    }
}
