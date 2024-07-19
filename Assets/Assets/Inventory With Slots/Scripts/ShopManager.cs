using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;
    public GameObject ShipInventorySlotPrefab;
    [HideInInspector] List<ShopInventorySlotController> slots = new List<ShopInventorySlotController>();
    public List<ShopItemMetadata> itemsInShop;
    public GameObject ShopContent;


    [System.Serializable]
    public class ShopItemMetadata
    {
        public Item item;
        public int price;
    }

    public void Awake()
    {
        Instance = this;
        AddItemToShop();
    }


    public void AddItemToShop()
    {
        // get a prefab for the ship inventory slot

        foreach(ShopItemMetadata itemMetadata in itemsInShop)
        {  
			GameObject newItemGameObject = Instantiate(ShipInventorySlotPrefab, ShopContent.transform);
			var shopInventoryController = newItemGameObject.GetComponent<ShopInventorySlotController>();

            shopInventoryController.InitializeItem(itemMetadata.item, itemMetadata.price);

            slots.Add(shopInventoryController);

		} 
    }


    public bool PurchaseItem(Item itemToPurchase, int price)
    {
        // does the player have enough money?
        var playerTotalMoney = InventoryManagerNew.Instance.GetMoney();

        if (playerTotalMoney < price) return false;

        // subtract the money 
        // add the item to the inventory
        var itemAdded = InventoryManagerNew.Instance.AddItem(itemToPurchase);

        if(itemAdded)
        { 
			InventoryManagerNew.Instance.AddMoney(-price);
            return true;
		}

        return false;

    }



}
