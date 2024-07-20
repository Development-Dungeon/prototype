using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;
    public GameObject ShopInventorySlotPrefab;
    public GameObject ShopSellInventorySlotPrefab;
    [HideInInspector] List<ShopInventorySlotController> slots = new List<ShopInventorySlotController>();
    public List<ShopItemMetadata> itemsInBuyShop;
    public List<ShopItemMetadata> sellableItemsList;
    public List<ShopSellInventoryController> itemsInSellShop;
    public GameObject ShopContent;
    public GameObject SellShopContent;
    public TMP_Text SellShopCointText;
    public Item money;


    [System.Serializable]
    public class ShopItemMetadata
    {
        [Header("both shops")]
        public Item item;

        [Header("buy shop only")]
        public int buyPrice;

        [Header("sell shop only")]
        public int sellPrice;
    }

    public void Awake()
    {
        Instance = this;
        AddItemToShop();
    }


    public void AddItemToShop()
    {

        foreach(ShopItemMetadata itemMetadata in itemsInBuyShop)
        {  
			GameObject newItemGameObject = Instantiate(ShopInventorySlotPrefab, ShopContent.transform);
			var shopInventoryController = newItemGameObject.GetComponent<ShopInventorySlotController>();

            shopInventoryController.InitializeItem(itemMetadata.item, itemMetadata.buyPrice);

            slots.Add(shopInventoryController);

		} 
    }

    public void RefreshCoinTest()
    {
        var playerMoneyAmount = InventoryManagerNew.Instance.GetMoney();
        SellShopCointText.text = "x" + playerMoneyAmount;
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

			PopulateSellShop();
            RefreshCoinTest();

            return true;
		}


        return false;

    }


    public bool SellItem(Item item, int priceOfItem)
    {
        var itemSold = InventoryManagerNew.Instance.RemoveItem(item);

        if (!itemSold)
            return false;

        // if the player does not have money, then add the first coin manually
        var playerTotalMoney = InventoryManagerNew.Instance.GetMoney();

        if(playerTotalMoney <= 0)
        {
            InventoryManagerNew.Instance.AddItem(money);
            priceOfItem -= 1;
		}

        InventoryManagerNew.Instance.AddMoney(priceOfItem);
	    RefreshCoinTest();

        return true;
    }


    public void PopulateSellShop()
    {
        // clear the previous sell shop
        foreach(var sellSlot in itemsInSellShop)
        {
            if (sellSlot == null) continue;
            Destroy(sellSlot.gameObject);
		}
        itemsInSellShop = new List<ShopSellInventoryController>();

        // get every item and quantity from the inventory
        var slots = InventoryManagerNew.Instance.GetInventorySlots();
        if (slots == null) return;
        // remove money slot from list
        var tempSlots = new List<InventorySlot>();

        foreach(var slot in slots)
        {
            var inventoryItem = slot.GetComponentInChildren<InventoryItem>();
            if (inventoryItem == null)
                continue;
            else if (inventoryItem.item == money)
                continue;
            else
                tempSlots.Add(slot);
		}

        slots = tempSlots;

        // init a slot inside the content
        foreach(var slot in slots)
        {  
            var inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            int? sellPrice = FindSellPriceForItem(inventoryItem.item);

            if (sellPrice == null)
                continue;

			GameObject newItemGameObject = Instantiate(ShopSellInventorySlotPrefab, SellShopContent.transform);
			var sellController = newItemGameObject.GetComponent<ShopSellInventoryController>();

            sellController.InitializeItem(inventoryItem.item, sellPrice.Value, 1); 

		    itemsInSellShop.Add(sellController);
		}
    }

    private int? FindSellPriceForItem(Item item)
    {

        if (sellableItemsList == null || sellableItemsList.Count <=0) return null;
        if (item == null) return null;

        // search inside the list for a reference to that item
        var itemMetadata = sellableItemsList.Find(entry => entry.item.Equals(item));

        if(itemMetadata == null) return null;

        return itemMetadata.sellPrice;

    }

}
