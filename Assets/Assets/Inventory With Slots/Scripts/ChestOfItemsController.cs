using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOfItemsController : MonoBehaviour
{

    [System.Serializable]
    public class ItemAndQuantity
    {
        public Item item;
        public int quantity;

    }

    public List<ItemAndQuantity> items;

    void Pickup()
    {
        // add each item one by one (that sounds bad for money)
        foreach(var itemAndQuantity in items)
        {
            // try and add the item and decrease the quantity
            while (itemAndQuantity.quantity > 0 )
            {  
				var itemAdded = InventoryManagerNew.Instance.AddItem(itemAndQuantity.item);

                if (itemAdded)
                    itemAndQuantity.quantity--;
                else
                    break; 
		    }

            // if quantity is 0 then remove it from the list of items
		}

        items = items.FindAll(item => item.quantity > 0);

        // if everything has been put in the inventory then destroy the game object
        // otherwise leave the game object with only the remaining items inside of it

        if (items.Count == 0)
            Destroy(gameObject);

    }

    public void Update()
    {
		if(Input.inputString != null) {
            if (Input.GetKeyDown(KeyCode.E)){
                AttemptPickup();
		    }
        }
    }

    private void AttemptPickup()
    {

        var playerReachScript = Camera.main.gameObject.GetComponentInChildren<PlayerReach>();
        if (playerReachScript == null || !playerReachScript.IsRaycastHit())
            return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit)) 
		{ 
            // check if this object is the one that i am pointing at
            if(hit.collider.gameObject.Equals(gameObject))
			    Pickup();
		}

    }


}
