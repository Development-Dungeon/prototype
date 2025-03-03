using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnKeyPress : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab; // The item to drop
        [Range(0f, 1f)] public float dropChance; // The chance to drop this item (0.0 to 1.0)
    }

    public List<DropItem> dropItems = new List<DropItem>();

    private Transform player; // Reference to the player's transform
    public float interactRange = 3f; // How close the player has to be to interact

    void Start()
    {
        // Find the player using the "Player" tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }
    }

    void Update()
    {
        if (player == null) return; // Exit if player wasn't found

        // Check if the player is within interaction range and presses the 'E' key
        if (Vector3.Distance(player.position, transform.position) <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            DropItemBasedOnChance();
            Destroy(gameObject); // Destroy the 'create' object after dropping an item
        }
    }

    void DropItemBasedOnChance()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeChance = 0f;

        foreach (DropItem item in dropItems)
        {
            cumulativeChance += item.dropChance;
            if (randomValue <= cumulativeChance)
            {
                // Instantiate the item at the position of the 'create' object
                Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                break; // Exit after dropping one item
            }
        }
    }
}
