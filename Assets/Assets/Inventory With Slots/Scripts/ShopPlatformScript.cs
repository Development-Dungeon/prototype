using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlatformScript : MonoBehaviour
{

    public List<ShopItemMetadata> shopBuySellConfig;


    void OnTriggerEnter(Collider otherObject)
    {
        ShopManager.Instance.OpenShop(shopBuySellConfig);
        
    }

    private void OnTriggerExit(Collider other)
    {
        ShopManager.Instance.CloseShop();
        
    }
}
