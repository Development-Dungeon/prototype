using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;
    public GameObject ShopInventorySlotPrefab;
    public GameObject ShopSellInventorySlotPrefab;
    [HideInInspector] List<ShopBuyInventorySlotController> slots = new List<ShopBuyInventorySlotController>();
    [HideInInspector] public List<ShopItemMetadata> _shopBuySellConfig;
    [HideInInspector] List<ShopSellInventoryController> itemsInSellShop = new List<ShopSellInventoryController>();
    public GameObject ShopContent;
    public GameObject SellShopContent;
    public TMP_Text SellShopCointText;
    public Item money;
    public GameObject ShopUI;

    public void Awake()
    {
        Instance = this;
    }


    public void PopulateBuyShop(List<ShopItemMetadata> shopBuySellConfig)
    {
        // clear out the current buy shop
        foreach(var buySlot in slots)
        {
            if (buySlot == null) continue;
            Destroy(buySlot.gameObject);
		}
        slots = new List<ShopBuyInventorySlotController>();

        // get a prefab for the ship inventory slot
        foreach(ShopItemMetadata itemMetadata in shopBuySellConfig)
        {
            if (!itemMetadata.isBuyable || itemMetadata.buyQuantity <= 0)
                continue;

			GameObject newItemGameObject = Instantiate(ShopInventorySlotPrefab, ShopContent.transform);
			var shopInventoryController = newItemGameObject.GetComponent<ShopBuyInventorySlotController>();

            shopInventoryController.InitializeItem(itemMetadata.item, itemMetadata.buyPrice, itemMetadata.buyQuantity);

            slots.Add(shopInventoryController);
		}
        EnableBuyTextForAffordableItems();
    }

    public void EnableBuyTextForAffordableItems()
    {
        var totalPlayerMoney = InventoryManagerNew.Instance.GetMoney();
        // iterate through each item in buys lot and enable/disable based on the amount of money the player has
        foreach(var buySlot in slots)
        {  
            if(totalPlayerMoney >= buySlot.priceOfItem)
            {
                buySlot.EnableBuyImageIcon();
		    }
            else 
		    {
                buySlot.DisableBuyImageIcon();
		    }
		}

    }

    public void RefreshCoinDisplay()
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
        var itemAddedToInventory = InventoryManagerNew.Instance.AddItem(itemToPurchase);

        if(itemAddedToInventory)
        { 
			InventoryManagerNew.Instance.AddMoney(-price);

			PopulateSellShop(_shopBuySellConfig);
            EnableBuyTextForAffordableItems();
            RefreshCoinDisplay();
            RemoveItemFromConfig(itemToPurchase, 1);


            return true;
		}

        return false;

    }

    private void RemoveItemFromConfig(Item itemToRemove, int quantity)
    {
        // search for the item inside the config
        if (_shopBuySellConfig == null || _shopBuySellConfig.Count <= 0)
            return;

        ShopItemMetadata entryToRemove = null;

        foreach(var entry in _shopBuySellConfig)
        {  
            if(entry.item == itemToRemove)
            {
                if(entry.buyQuantity == -1)
                {  
                    // this item can be purchased infinitly so we don't have to do anything to the config
				}
                else if(entry.buyQuantity >= quantity)
                {  
					entry.buyQuantity -= quantity;
                    if(entry.buyQuantity == 0 && !entry.isSellable)
                    {
                        entryToRemove = entry;
                    }
				}
                else {
                    throw new System.Exception("Logical error : trying to remove a quantity from the buysellconfig greater then the quantity in the config. item (" + itemToRemove.itemName + ") quantity (" + quantity + ")");
				}
		    } 
		}

        if (entryToRemove != null)
            _shopBuySellConfig.Remove(entryToRemove);
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
	    RefreshCoinDisplay();

        return true;
    }


    public void PopulateSellShop(List<ShopItemMetadata> shopBuySellConfig)
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
        // remove money and empty slots from list
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

            int? sellPrice = FindSellPriceForItem(inventoryItem.item, shopBuySellConfig);

            if (sellPrice == null)
                continue;

			GameObject newItemGameObject = Instantiate(ShopSellInventorySlotPrefab, SellShopContent.transform);
			var sellController = newItemGameObject.GetComponent<ShopSellInventoryController>();

            sellController.InitializeItem(inventoryItem.item, sellPrice.Value, inventoryItem.itemCount); 

		    itemsInSellShop.Add(sellController);
		}
    }

    private int? FindSellPriceForItem(Item item, List<ShopItemMetadata> shopBuySellConfig)
    {

        if (shopBuySellConfig == null || shopBuySellConfig.Count <=0) return null;
        if (item == null) return null;

        // search inside the list for a reference to that item
        var itemMetadata = shopBuySellConfig.Find(entry => entry.item.Equals(item));

        if(itemMetadata == null || !itemMetadata.isSellable) return null;

        return itemMetadata.sellPrice;

    }

    public void OpenShop(List<ShopItemMetadata> shopBuySellConfig)
    {
        _shopBuySellConfig = shopBuySellConfig;
        ShopUI.SetActive(true);
        PopulateBuyShop(shopBuySellConfig);

        // saving list here so that when buying an item you can see it populate inside the sell list window. 
        // saving the variable here is a little odd. Leveraging events could resolve this concern. Until then saving a copy of the list 
        PopulateSellShop(shopBuySellConfig);
    }

    public void CloseShop()
    {  
        ShopUI.SetActive(false);
    }

}
