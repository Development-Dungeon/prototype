using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("UI")]
    public Image image;
    public TMP_Text countText;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public ItemNew item;
    [HideInInspector] public int itemCount = 0;


    public void InitialiseItem(ItemNew newItem) 
    {
        item = newItem;
        image.sprite = newItem.image;
        itemCount = ++itemCount;
        RefreshCount();
    }

    public bool AddItem(ItemNew itemToAdd)
    {  
        if(itemToAdd != item)
            throw new Exception("Cannot add item to inventory slot because they are not the same. Current item in slot : " + item.itemName + " item being added: " + itemToAdd.itemName );

        if (itemCount >= itemToAdd.maxStackable )
            return false;

	    itemCount = ++itemCount;
	    RefreshCount();

        return true;
    }

    public void RemoveItem()
    {
        itemCount--;
        if (itemCount <= 0)
            Destroy(this.gameObject);
        else
            RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = "x" + itemCount.ToString();
        bool textActive = itemCount > 1;

        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
