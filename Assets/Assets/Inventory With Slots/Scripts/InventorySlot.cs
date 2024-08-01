using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot: MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public List<ItemType> onlyAllowTypes;
    public List<ItemType> notAllowTypes;

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
			dragableItem.parentAfterDrag = transform;
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
