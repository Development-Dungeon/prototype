using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This script what is displayed in the shop inventory slot
public class ShopInventorySlotController : MonoBehaviour
{

    public Item item;
    public Image itemImage;
    public Image buyBackgroundImage;
    public TMP_Text priceText;
    public Color enabledColor, disabledColor;
    public int priceOfItem;

    public void InitializeItem(Item _item, int price)
    {
        item = _item;
        itemImage.sprite = item.image;
        priceOfItem = price;
        RefreshPrice();
        buyBackgroundImage.color = disabledColor;
    }

    public void RefreshPrice()
    {
        priceText.text = "X" + priceOfItem;
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
            DisableBuyImageIcon();


    }


}
