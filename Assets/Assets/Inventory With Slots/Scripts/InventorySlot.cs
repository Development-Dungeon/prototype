using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class InventorySlot: MonoBehaviour, IDropHandler
{
    public Image image;
    public bool IsEquipmentSlot;
    public Color selectedColor, notSelectedColor;
    public List<ItemType> onlyAllowTypes;
    public List<ItemType> notAllowTypes;

    public static event Action<Item> EquipmentAdded;
    public static event Action<Item> EquipmentRemoved;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // get the number of inventory items and checking that count
        var itemInSlot = this.gameObject.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
            return;

		GameObject dropped = eventData.pointerDrag;
		var dragableItem = dropped.GetComponent<InventoryItem>();

        if (dragableItem == null)
            return;

        // make sure that the inventory to drop to supports the type
        if (ItemAllowedInSlot(dragableItem.item.type))
        {
            Transform previousParent = dragableItem.parentAfterDrag;

            dragableItem.parentAfterDrag = transform;


            // if it is an equipment slot then send a message out that the item was added
            if(IsEquipmentSlot)
            {
                if (EquipmentAdded != null)
                    EquipmentAdded.Invoke(dragableItem.item);
		    }
            else
            {
                // check where the previous parent was and if it was an equipment slot
                var previousInventorySlot = previousParent.GetComponent<InventorySlot>();
                if(previousInventorySlot == null)
                {
                    Debug.Log("On drop looking expecting previous parent to be an inventory slot but it has returned null. This is unexpected and you should look at why this is happening");
                    return;
				}
                if(previousInventorySlot.IsEquipmentSlot)
                    EquipmentRemoved.Invoke(dragableItem.item);
                
		    }
        }
        
    }

    public bool ItemAllowedInSlot(ItemType itemType)
    {  
        // if the only list is configured then ignore the disallow
        if(onlyAllowTypes != null && onlyAllowTypes.Count > 0) 
		{
            return onlyAllowTypes.Contains(itemType);
		}

        if(notAllowTypes != null && notAllowTypes.Count > 0)
        {
            return !notAllowTypes.Contains(itemType);
		}

        return true;
    }
}
