using UnityEngine;

public class LayeredFog : MonoBehaviour
{
    [System.Serializable]
    public class FogLayer
    {
        public float yPosition; // Y position for transitioning
        public Color color;
        public float density;
    }

    public FogLayer[] fogLayers; // Array to hold fog layers
    public Transform player;     // Reference to the player's transform
    public float gizmoWidth = 50f; // Width for the gizmo boxes in the scene
    public float transitionSpeed = 0.5f; // Speed of the fog transition

    private Color currentFogColor;
    private float currentFogDensity;
    private Color targetFogColor;
    private float targetFogDensity;

    private const float DefaultY = 552.3f; // Default fog level

    void LateUpdate()
    {
        if (fogLayers.Length == 0 || player == null) return;

        float playerY = player.position.y;

        // Initialize target values
        Color defaultColor = fogLayers[0].color;
        float defaultDensity = fogLayers[0].density;

        // Default values if playerY is above the default Y value
        targetFogColor = (playerY >= DefaultY) ? defaultColor : defaultColor;
        targetFogDensity = (playerY >= DefaultY) ? defaultDensity : defaultDensity;

        // Find the current layer based on the player's y-position if below DefaultY
        if (playerY < DefaultY)
        {
            for (int i = 0; i < fogLayers.Length; i++)
            {
                var layer = fogLayers[i];
                if (playerY >= layer.yPosition)
                {
                    targetFogColor = layer.color;
                    targetFogDensity = layer.density;
                }
            }
        }

        // Smoothly interpolate towards the target values
        currentFogColor = Color.Lerp(currentFogColor, targetFogColor, Time.deltaTime * transitionSpeed);
        currentFogDensity = Mathf.Lerp(currentFogDensity, targetFogDensity, Time.deltaTime * transitionSpeed);

        // Apply the fog settings
        RenderSettings.fog = true;
        RenderSettings.fogColor = currentFogColor;
        RenderSettings.fogDensity = currentFogDensity;
    }

    // Draw gizmos in the Scene view to visualize fog layers at the player's position
    private void OnDrawGizmos()
    {
        if (fogLayers == null || fogLayers.Length == 0 || player == null) return;

        Vector3 playerPos = player.position;

        // Loop through each fog layer and draw its yPosition at the player's position
        foreach (var layer in fogLayers)
        {
            // Set the gizmo color to the fog layer's color
            Gizmos.color = new Color(layer.color.r, layer.color.g, layer.color.b, 0.5f); // Semi-transparent

            // Draw a box representing the fog layer range in the Y direction, centered at the player's position
            Vector3 center = new Vector3(playerPos.x, layer.yPosition, playerPos.z); // Center of the layer's Y position at player position
            Vector3 size = new Vector3(gizmoWidth, 0.1f, gizmoWidth); // Thin box representing the layer's Y position

            Gizmos.DrawCube(center, size);

            // Draw the line for yPosition
            Gizmos.color = layer.color;
            Gizmos.DrawLine(new Vector3(playerPos.x - gizmoWidth / 2, layer.yPosition, playerPos.z),
                            new Vector3(playerPos.x + gizmoWidth / 2, layer.yPosition, playerPos.z)); // yPosition line
        }
    }
}
