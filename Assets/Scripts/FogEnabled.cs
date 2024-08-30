using UnityEngine;

public class ForceFog : MonoBehaviour
{
    public Color fogColor;
    public float fogDensity = 0.2f;

    void LateUpdate()
    {
        // Ensure the fog system is enabled
        RenderSettings.fog = true;

        // Set the fog color and density
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
    }
}
