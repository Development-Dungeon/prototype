using UnityEngine;

public class EnableDisableByHeight : MonoBehaviour
{
    public GameObject objectToToggle; // The game object you want to enable/disable
    public Transform player;          // Reference to the player's transform
    public float heightThreshold = 552f; // The y height threshold

    void Update()
    {
        // Check the player's y position
        if (player.position.y > heightThreshold)
        {
            // If player's y is more than 552, disable the game object
            objectToToggle.SetActive(false);
        }
        else
        {
            // If player's y is less than or equal to 552, enable the game object
            objectToToggle.SetActive(true);
        }
    }
}
