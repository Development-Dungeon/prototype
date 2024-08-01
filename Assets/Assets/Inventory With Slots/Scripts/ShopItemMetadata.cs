using UnityEngine;

[System.Serializable]
public class ShopItemMetadata
{
    [Header("both shops")]
    public Item item;

    [Header("buy shop only")]
    public bool isBuyable;
    public int buyPrice;
    public int buyQuantity;

    [Header("sell shop only")]
    public bool isSellable;
    public int sellPrice;
}

