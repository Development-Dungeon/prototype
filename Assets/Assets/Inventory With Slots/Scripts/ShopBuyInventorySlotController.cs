using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This script what is displayed in the shop inventory slot
public class ShopBuyInventorySlotController : MonoBehaviour
{
    [HideInInspector] public Item item;
    public Image itemImage;
    public Image buyBackgroundImage;
    public TMP_Text priceText;
    public TMP_Text quantityText;
    public Color enabledColor, disabledColor;
    [HideInInspector] public int priceOfItem;
    [HideInInspector] public int quantity;

    // when quanity is -1, then the item can be purchased an unlimited amount of times 
    private static int UNLIMITED_BUY_QUANTIY = -1;

    public void InitializeItem(Item _item, int price, int _quantity)
    {
        item = _item;
        itemImage.sprite = item.image;
        priceOfItem = price;
        quantity = _quantity;
        RefreshPrice();
        RefreshQuantity();
        buyBackgroundImage.color = disabledColor;
    }

    public void RefreshPrice()
    {
        priceText.text = "X" + priceOfItem;
    }
    public void RefreshQuantity()
    {
        if(quantity == UNLIMITED_BUY_QUANTIY)
			quantityText.text = "";
        else  
			quantityText.text = "x" + quantity;
    }

    public void EnableBuyImageIcon()
    {
        buyBackgroundImage.color = enabledColor;
    }

    public void DisableBuyImageIcon()
    {
        buyBackgroundImage.color = disabledColor;
    }


    public void PurchaseItem()
    {
        var itemPurchased = ShopManager.Instance.PurchaseItem(item,priceOfItem);

        if (itemPurchased)
        {
            if(quantity == UNLIMITED_BUY_QUANTIY)
            {  
                // do nothing. the item can continue to be purchased
		    }
            else
            {  
			    quantity--;
                if(quantity == 0)
                    Destroy(gameObject);
		    }

		}

    }


}
