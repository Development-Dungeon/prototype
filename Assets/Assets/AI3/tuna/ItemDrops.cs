using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDrops : MonoBehaviour
{

    public bool singleItemDrop;

    [System.Serializable]
    public class ItemDropMetadata 
    {
        public GameObject itemPrefab;
        [Range(0f, 1f)] public float dropChance; 
    }

    public List<ItemDropMetadata> itemsToDrop;

    private void Awake()
    {
        GetComponent<Health>().HealthPercentChangeEvent += DropItems;
    }


    private void OnDestroy()
    {
        GetComponent<Health>().HealthPercentChangeEvent -= DropItems;
    }

    private void DropItems(float healthPercent)
    {
        if (healthPercent > 0) return;

        if (itemsToDrop == null || itemsToDrop.Count == 0) return;

        float selectedRate = Random.Range(0f, 1f);
        List<ItemDropMetadata> possibleDroppedItems = itemsToDrop.OrderBy(i => i.dropChance).ToList().FindAll((i) => selectedRate <= i.dropChance).ToList();

        if (possibleDroppedItems.Count == 0) return;

        ItemDropMetadata selectedItem = null;

        if (singleItemDrop)
        {
            selectedItem = possibleDroppedItems.First();
            Instantiate(selectedItem.itemPrefab, transform.position, transform.rotation);

        }
        else if (!singleItemDrop)
        {
            possibleDroppedItems.ForEach(i => Instantiate(i.itemPrefab, transform.position, transform.rotation));
        }
    }
}
