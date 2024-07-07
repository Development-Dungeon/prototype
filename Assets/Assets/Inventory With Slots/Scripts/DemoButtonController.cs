using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoButtonController : MonoBehaviour
{

    public ItemNew itemToCreate;

    public void addItem()
    {  
        InventoryManagerNew.Instance.AddItem(itemToCreate);

    }
   
}
