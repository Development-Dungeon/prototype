using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public int additionalRange = 0;

    void Pickup()
    {
        var itemAdded = InventoryManagerNew.Instance.AddItem(item);

        if(itemAdded) {  
			Destroy(gameObject);
		}
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
        if (playerReachScript == null || !playerReachScript.IsRaycastHit(additionalRange))
            return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit)) 
		{ 
            if(hit.collider.gameObject.Equals(gameObject))
			    Pickup();
		}
    }

}
