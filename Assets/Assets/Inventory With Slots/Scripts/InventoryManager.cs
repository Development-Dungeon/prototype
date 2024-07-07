using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerNew : MonoBehaviour
{
    public InventorySlotNew[] inventorSlots;
    public GameObject inventoryObjectPrefab;
    public static InventoryManagerNew Instance;
    public int selectedSlot = -1;


    public void Start()
    {
        ChangedSelectedSlot(0);
    }
    private void Update()
    {
        if(Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 8) { 
                ChangedSelectedSlot(number - 1);
		    }
            else if (Input.inputString.Equals("q")) {
                RemoveSelectedItem();
		    }
            else if (Input.inputString.Equals("l")) {
                DropItemInWorld();
		    }
        }
    }

    private void DropItemInWorld()
    {
        // get the currently selected item
        var currentlySelectedItem = GetSelectedItem();
        if (currentlySelectedItem == null)
            return;

        // spawn its 3d prefab at the players current reach distance 
        var prefab = currentlySelectedItem.inWorldPrefab;

        var playerLocation = Camera.main.transform;
		var playerReachScript = Camera.main.gameObject.GetComponentInChildren<PlayerReach>();

        if(playerReachScript == null)
            return;

        var reachDistance = playerReachScript.reachDistance;
        var inFrontOfPlayer = playerLocation.position + (playerLocation.transform.forward * reachDistance);

        Instantiate(prefab, inFrontOfPlayer, playerLocation.rotation);

        // remove the object from the inventory
        RemoveSelectedItem();

    }

    void ChangedSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
			inventorSlots[selectedSlot].Deselect();

        selectedSlot = newValue;
        inventorSlots[selectedSlot].Select();
    }

    public void Awake()
    {
        Instance = this;
    }

    public bool AddItem(ItemNew item)
    {
        
	    var hasAddedItemToStack = AddStackableItem(item);
	    if (hasAddedItemToStack)
			return true;

        var hasAddedItemToEmptySlot = AddItemToEmptySlot(item);
	    if (hasAddedItemToEmptySlot)
			return true;

        return false;

    }

    private bool AddItemToEmptySlot(ItemNew item)
    {  
        var emptySlots = FindAllEmptySlots();

        for(int i = 0; i < emptySlots.Length; i++) {

            var slot = emptySlots[i];
			SpawnNewItem(item, slot);

			return true;
		}

        return false;

    }

    private bool AddStackableItem(ItemNew item)
    {
		// search for the item to see if there exists an entry inside the inventory which contains the item already
        // if the item is found then check if the max stackable amount is reached
        // if the max count is reached then continue searching for another inventory slot to add item to
        if (item == null)
            return false;

        if (!item.stackable)
            return false;

        var slotsWithMatchingItem = FindAllSlotsWith(item);

        for (int i = 0; i < slotsWithMatchingItem.Length; ++i)
        {
            var slot = slotsWithMatchingItem[i];
            var inventoryItem = slot.GetComponentInChildren<InventoryItem>();

			var successfullyAddedItem = inventoryItem.AddItem(item);

            if (successfullyAddedItem)
                return true;
            else
                continue;
        }

        return false;
    }

    private InventorySlotNew[] FindAllEmptySlots()
    {
        var emptySlots = new List<InventorySlotNew>();

        for (int i = 0; i < inventorSlots.Length; i++)
        {

            var slot = inventorSlots[i];
            var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                emptySlots.Add(slot);
            }
        }

        return emptySlots.ToArray();
    }

    private InventorySlotNew[] FindAllSlotsWith(ItemNew item)
    {
        if (item == null)
            return new InventorySlotNew[0];

        var foundSlots = new List<InventorySlotNew>();

        for (int i = 0; i < inventorSlots.Length; i++)
        {
            var slot = inventorSlots[i];
            var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                foundSlots.Add(slot);
            }
        }

        return foundSlots.ToArray();

    }


    void SpawnNewItem(ItemNew item, InventorySlotNew slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryObjectPrefab, slot.transform);
        var inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public ItemNew GetSelectedItem()
    {
        var slot = inventorSlots[selectedSlot];
        var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null)
            return null;
        return itemInSlot.item;
    } 

    public void RemoveSelectedItem()
    {
        var slot = inventorSlots[selectedSlot];
        var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null)
            return ;

        // if the item has multiple, decrease the count
        // if the item is a single then delete the inventory item
	    itemInSlot.RemoveItem();


    }
   
}
