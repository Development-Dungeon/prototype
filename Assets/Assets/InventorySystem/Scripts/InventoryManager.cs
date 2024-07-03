using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public int MaxInventorySize = 5;

    public Transform ItemContent;
    public GameObject InventoryItem;
    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;

    private void Awake()
    {
        Instance = this;

    }

    public bool Add(Item item)
    {
        if (IsInventoryFull())
            return false;

		Items.Add(item);
		RefreshOpenInventory();

	    return true;
    }

    public bool IsInventoryFull()
    {
        return Items.Count >= MaxInventorySize;
    }

    public void RefreshOpenInventory()
    {
        if (!ItemContent.gameObject.activeInHierarchy)
        {
            return;
        }

        ClearContent();
        ListItems();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ClearContent()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        if (InventoryItem == null || InventoryItems.Length < 1)
            return;


        for (int i = 0; i < InventoryItems.Length; ++i)
        {
            Destroy(InventoryItems[i].gameObject);
        }

    }

    public void ListItems()
    {

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemImage").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();
            var itemControllerScript = obj.GetComponent<InventoryItemController>();

            itemControllerScript.AddItem(item);
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if (EnableRemove.isOn)
            {
                removeButton.gameObject.SetActive(true);
            }

        }

    }

    public void EnableItemsRemove()
    {
        foreach (Transform item in ItemContent)
        {
            item.Find("RemoveButton").gameObject.SetActive(EnableRemove.isOn);
        }

    }
}