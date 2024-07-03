using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    void Pickup()
    {
        var itemAdded = InventoryManager.Instance.Add(item);

        if(itemAdded) {  
			Destroy(gameObject);
		}
    }

    private void OnMouseDown()
    {
        Pickup();
    }

}
