using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    void Pickup()
    {
        var itemAdded = InventoryManagerNew.Instance.AddItem(item);

        if(itemAdded) {  
			Destroy(gameObject);
		}
    }

    private void OnMouseDown()
    {
        // get the player reach
        var playerReachScript = Camera.main.gameObject.GetComponentInChildren<PlayerReach>();
        if(playerReachScript == null)
            return;

        var reachDistance = playerReachScript.reachDistance;

        if (Math.Abs(Vector3.Distance(this.transform.position, Camera.main.transform.position)) < reachDistance)
        {
			Pickup();
        }
    }

}
