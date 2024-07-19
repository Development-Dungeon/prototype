using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ShopSellInventoryController : MonoBehaviour
{
	public Item item;
    public Image itemImage;
    public TMP_Text priceText;
    public TMP_Text quantityText;
    public int priceOfItem;
    public int quantity;

    public void InitializeItem(Item _item, int price, int _quantity)
    {
        item = _item;
        itemImage.sprite = item.image;
        priceOfItem = price;
        quantity = _quantity;
        RefreshPrice();
        RefreshQuantity();
    }

    public void RefreshPrice()
    {
        priceText.text = "X" + priceOfItem;
    }

    public void RefreshQuantity()
    {
        quantityText.text = "X" + quantity;
    }

    public void SellItem()
    {
        // assume the ShopManager will update the inventory 
        var itemSold = ShopManager.Instance.SellItem(item, priceOfItem);

        if(itemSold) 
	        UpdateQuantityText(-1);

        if(quantity <= 0)
            Destroy(gameObject);
    }

    public void UpdateQuantityText(int quantityUpdateAmount)
    {
        quantity += quantityUpdateAmount;
        RefreshQuantity();
    }
}
