using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable 
{
    public Item item;
    
    public void Pickup()
    {
        var itemAdded = InventoryManagerNew.Instance.AddItem(item);

        if(itemAdded) {  
			Destroy(gameObject);
		}
    }
}
