using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlatformScript : MonoBehaviour
{

    public GameObject ShopUI;

    void OnTriggerEnter(Collider otherObject)
    {
        // open shop inventory
        ShopUI.SetActive(true);
        ShopManager.Instance.PopulateSellShop();
    }

    private void OnTriggerExit(Collider other)
    {
        // close shop inventory
        ShopUI.SetActive(false);

    }
}
