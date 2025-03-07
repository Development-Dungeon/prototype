using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManagerNew : MonoBehaviour
{
    public InventorySlot[] inventorSlots;

    public InventorySlot tankSlot;
    public InventorySlot HeadSlot;
    public InventorySlot ChestSlot;
    public InventorySlot SuitSlot;

    public GameObject inventoryObjectPrefab;
    public static InventoryManagerNew Instance;
    public int selectedSlot = -1;
    public GameObject mainInventory;
    public Item money;

    [HideInInspector]
    public bool IsInventoryOpen = false;

    public List<InventorySlot> GetInventorySlots() { return new List<InventorySlot>(inventorSlots); }

    public static event Action<Item> SelectedItemChanged;
    

    public int GetMoney()
    {
        // search for the inventory slot and return the amount
        var slotsWithMoney = FindAllSlotsWith(money);

        int totalMoney = 0;

        if (slotsWithMoney == null)
            return totalMoney;


        foreach(var slot in slotsWithMoney)
	    {

            var inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            if (inventoryItem == null) continue;

            totalMoney += inventoryItem.itemCount;
		}


        return totalMoney;
    }

    internal void AddMoney(int moneyToAdd)
    {
        var totalMoney = GetMoney();
        var remaningMoney = totalMoney + moneyToAdd;

        // make sure that the player has money
        // if the player does not have enough money then throw an error
        if(remaningMoney < 0)
            throw new System.Exception("Trying to subtract more money then the palyer has");

        if(remaningMoney > money.maxStackable)
            throw new System.Exception("Trying to add more money then is allowed to be stacked");

        // if the player does have enough money then subtract the count from the money slot
        // get the money slot and subtract
        var moneySlots = FindAllSlotsWith(money);
        var playerMoneyItem = moneySlots[0].GetComponentInChildren<InventoryItem>();

        playerMoneyItem.AddToExistingItemQuantity(moneyToAdd);

    }

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
            else if (Input.GetKeyDown(KeyCode.Q)){
                RemoveSelectedItem();
		    }
            else if (Input.GetKeyDown(KeyCode.G)) {
                DropItemInWorld();
		    } 
            else if (Input.GetKeyDown(KeyCode.Tab)) {
                ToggleInventory();
		    }
        }
    }

    public bool RemoveItem(Item item)
    {
        var slots = FindAllSlotsWith(item);

        if (slots == null || slots.Length == 0)
            return false;

        var slot = slots[0];
        var itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot == null)
            throw new System.Exception("attempting to remove item but cannot find the inventory item component in childreen of slot");

	    itemInSlot.RemoveItem();

        return true;

    }

    private void ToggleInventory()
    {
        IsInventoryOpen = !IsInventoryOpen;
        mainInventory.SetActive(IsInventoryOpen);

        if (IsInventoryOpen)
        {  
		    Cursor.lockState = CursorLockMode.None;
		    Cursor.visible = true;
            FirstPersonController.cameraCanMove = false;
		}
        else {  
		    Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            FirstPersonController.cameraCanMove = true;
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

        if (SelectedItemChanged != null)
            SelectedItemChanged.Invoke(GetSelectedItem());


    }

    public void Awake()
    {
        Instance = this;
    }

    public bool AddItem(Item item)
    {
        
	    var hasAddedItemToStack = AddStackableItem(item);
	    if (hasAddedItemToStack)
			return true;

        var hasAddedItemToEmptySlot = AddItemToEmptySlot(item);
	    if (hasAddedItemToEmptySlot)
			return true;

        return false;

    }

    private bool AddItemToEmptySlot(Item item)
    {  
        var emptySlots = FindAllEmptySlots();

        for(int i = 0; i < emptySlots.Length; i++) {

            var slot = emptySlots[i];

            if (slot.ItemAllowedInSlot(item.type)) 
		    { 
				SpawnNewItem(item, slot);
				return true;
		    }
		}

        return false;

    }

    private bool AddStackableItem(Item item)
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

    private InventorySlot[] FindAllEmptySlots()
    {
        var emptySlots = new List<InventorySlot>();

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

    private InventorySlot[] FindAllSlotsWith(Item item)
    {
        if (item == null)
            return new InventorySlot[0];

        var foundSlots = new List<InventorySlot>();

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


    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryObjectPrefab, slot.transform);
        var inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem()
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
