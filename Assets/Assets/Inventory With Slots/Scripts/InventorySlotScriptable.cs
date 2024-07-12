using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Slot", menuName = "Inventory Slot new/Create new Inventory Slot")]
public class InventorySlotScriptable : ScriptableObject 
{
    public Type type;
    public bool isStackable;
    public int maxQuantity;

    public enum Type {  
        ACTIVE_BAR,
	    INVENTORY, 
	    HELMET,
	    TANK 

    }

}
