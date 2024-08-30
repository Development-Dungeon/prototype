using UnityEngine;

public class LayeredFog : MonoBehaviour
{
    [System.Serializable]
    public class FogLayer
    {
        public float minY;
        public float maxY;
        public Color color;
        public float startDensity;
        public float endDensity;
    }

    public FogLayer[] fogLayers; // Array to hold fog layers
    public Transform player;     // Reference to the player's transform

    private Color currentFogColor;
    private float currentFogDensity;

    void LateUpdate()
    {
        if (fogLayers.Length == 0 || player == null) return;

        float playerY = player.position.y;

        // Initialize values
        currentFogColor = fogLayers[0].color;
        currentFogDensity = fogLayers[0].startDensity; // Start density of the first layer

        // Find the correct layer based on the player's y-position
        for (int i = 0; i < fogLayers.Length - 1; i++)
        {
            var layer = fogLayers[i];
            var nextLayer = fogLayers[i + 1];

            if (playerY >= layer.minY && playerY <= nextLayer.maxY)
            {
                float t = (playerY - layer.minY) / (nextLayer.maxY - layer.minY);

                // Interpolate color and density
                currentFogColor = Color.Lerp(layer.color, nextLayer.color, t);
                currentFogDensity = Mathf.Lerp(layer.startDensity, nextLayer.endDensity, t);
                break;
            }
        }

        // Apply the fog settings
        RenderSettings.fog = true;
        RenderSettings.fogColor = currentFogColor;
        RenderSettings.fogDensity = currentFogDensity;
    }
}
