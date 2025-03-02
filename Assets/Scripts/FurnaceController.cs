using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FurnaceController : MonoBehaviour
{
    
    public GameObject furnaceUIPrefab;
    public GameObject furnaceUI;
    public TMP_Text consumptionRateText; 
    public TMP_Text heatProductionText;
    public TMP_Text furnaceIsOnText;
    public TMP_Text progressText;
    public float furnaceMaxTemperature;
    public float furnaceItemConsumptionRateInSeconds;
    private Utilities.CountdownTimer consumeItemCountdownTimer;
    private InventorySlot inventorySlot;
    
    // I need to have an event that comes back when an item has been dropped or removed so that I know when to start the timers
    
    /*
     * I need to open the UI when the player collides with the box
     */
    void OnTriggerEnter(Collider otherObject)
    {
        // Open the UI
        OpenFurnaceUI();
        PopulateFurnaceUI();
        UnlockCursor();
    }

    private void UnlockCursor()
    {
        FirstPersonController.cameraCanMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;    
    }
    
    private void LockCursor()
    {
        FirstPersonController.cameraCanMove = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;    
    }

    private void OnTriggerExit(Collider other)
    {
        // Close the UI
        CloseFurnaceUI();
        LockCursor();
    }

    private void PopulateFurnaceUI()
    {
       heatProductionText.text = furnaceMaxTemperature + "F";
       consumptionRateText.text = furnaceItemConsumptionRateInSeconds + "s";
       furnaceIsOnText.text = "" + consumeItemCountdownTimer.IsRunning;
       
       if (consumeItemCountdownTimer.IsRunning)
           progressText.text = consumeItemCountdownTimer.Progress.ToString("P0");
       else
           progressText.text = "0%";

    }

    public void CloseFurnaceUI()
    {
        if (furnaceUI == null)
            return;
        
        furnaceUI.SetActive(false);
    }

    public void OpenFurnaceUI()
    {
        if (furnaceUI == null)
            return;
        
        furnaceUI.SetActive(true);
        
    }
    
    /*
     * this code needs to do the following
     * 1) See that there is an item inside the inventory slot
     * 2) Pull the power from the item
     * 3) consume the item
     * 4) update the text for the furnace status
     */
    
    void Start()
    {
        consumeItemCountdownTimer = new Utilities.CountdownTimer(0);
        consumeItemCountdownTimer.OnTimerStop += ConsumeItem; 
        
        inventorySlot = furnaceUI.GetComponentInChildren<InventorySlot>();
        inventorySlot.ItemAdded += ItemAdded;
    }

    private void ResetTimer()
    {
        consumeItemCountdownTimer.Reset(furnaceItemConsumptionRateInSeconds);
    }

    private void ItemAdded(InventoryItem item)
    {
        if (consumeItemCountdownTimer.IsRunning)
            return;
        ConsumeItem(item);
    }
    
    private void ConsumeItem(InventoryItem heldItem)
    {
        // get the item which is in the inventory slot
        
        if (heldItem == null)
        {
            return;
        } 
        
        // delete one of the items
        heldItem.RemoveItem(1);
        
        consumeItemCountdownTimer.Reset(furnaceItemConsumptionRateInSeconds);
        consumeItemCountdownTimer.Start();
    }

    // The issue with this method is that during the drag and drop process the
    // parent change does not occur in the same frame as the invocation. Therefore, inventory item cannot be found right away.
    // For that reason, I had to duplicate the method with the item as parameter. 
    // its possible that I need to add an additional delay when adding items so that this parent process can complete.
    // The below can potentially be used in another flow
    private void ConsumeItem()
    {
        // get the item which is in the inventory slot
        InventoryItem heldItem = furnaceUI.GetComponentInChildren<InventoryItem>();
        
        if (heldItem == null)
        {
            return;
        } 
        
        heldItem.RemoveItem(1);
        
        consumeItemCountdownTimer.Reset(furnaceItemConsumptionRateInSeconds);
        consumeItemCountdownTimer.Start();

    }

    void Update()
    {
        consumeItemCountdownTimer.Tick(Time.deltaTime);
        if(furnaceUI.activeSelf)
            PopulateFurnaceUI();
    }
}
