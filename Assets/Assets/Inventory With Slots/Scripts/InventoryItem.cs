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

    [HideInInspector] 
    public Transform parentAfterDrag;
    [HideInInspector] 
    public Item item;
    [HideInInspector] 
    public int itemCount = 0;


    public void InitialiseItem(Item newItem) 
    {
        item = newItem;
        image.sprite = newItem.image;
        itemCount = ++itemCount;
        RefreshCount();
    }

    public bool AddItem(Item itemToAdd)
    {  
        if(itemToAdd != item)
            throw new Exception("Cannot add item to inventory slot because they are not the same. Current item in slot : " + item.itemName + " item being added: " + itemToAdd.itemName );

        if (itemCount >= itemToAdd.maxStackable )
            return false;

	    itemCount = ++itemCount;
	    RefreshCount();

        return true;
    }

    public void AddToExistingItemQuantity(int numberOfItemsToAdd)
    {  
        itemCount += numberOfItemsToAdd;
        if (itemCount <= 0)
            Destroy(this.gameObject);
        else
            RefreshCount();
    }

    public void RemoveItem(int numberOfItemsToRemove)
    {
        itemCount -= numberOfItemsToRemove;
        if (itemCount <= 0)
            Destroy(this.gameObject);
        else
            RefreshCount();
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
        ToggleOverlay(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // get the parent OverlayController and set the overlay to true
        ToggleOverlay(true);

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

        ToggleOverlay(false);
		
    }

    private void ToggleOverlay(bool enable)
    {  
		var itemOverlayController = transform.parent.GetComponentInChildren<ItemOverlayController>();

        if (itemOverlayController == null)
            return;

        if (enable)
            itemOverlayController.EnableOverlay();
        else 
		    itemOverlayController.DisableOverlay();

    }
}
