using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

// convert this code to only use the prefab and not dragging all the information
public class FurnaceController : MonoBehaviour
{ public GameObject furnaceUIPrefab;
    public GameObject furnaceUI;
    public GameObject furnaceUIParent; // This object will contain all the furnace UI's when they are created
    // public float furnaceMaxTemperature;
    public float baseFurnaceItemConsumptionRateInSeconds;
    
    private Utilities.CountdownTimer consumeItemCountdownTimer;
    private InventorySlot inventorySlot;
    private TMP_Text consumptionRateText; 
    private TMP_Text heatProductionText;
    private TMP_Text furnaceIsOnText;
    private TMP_Text progressText;
    private HeatSourceScript targetHeatSource;
    private float lastConsumedItemBurnRateInSeconds;
    
    void OnTriggerEnter(Collider otherObject)
    {
        InitFurnacePrefab();
        InitHeatSource();
        OpenFurnaceUI();
        PopulateFurnaceUI();
        UnlockCursor();
    }

    private void InitHeatSource()
    {
        if (targetHeatSource != null)
            return;
        
        targetHeatSource = GetComponent<HeatSourceScript>();
        if (targetHeatSource == null)
        {
           Debug.LogError("Expecting a heat source on furnace object but none found. Please add a heat source script to the furnace object.");
           return;
        }

        targetHeatSource.isActive = false;
    }

    private void InitFurnacePrefab()
    {
        if (furnaceUI != null)
            return;
        furnaceUI = Instantiate(furnaceUIPrefab, furnaceUIParent.transform, true);
        // get the ui components script
        var furnaceUIComponents = furnaceUI.GetComponent<FurnaceUIComponents>();
        
        // the assumption is that this is not going to be null
        // if it is null i should throw an error
        if (furnaceUIComponents == null)
        {
            Debug.LogError("The furnace prefab does not have a FurnaceUIComponents component which is unexpected. Please add it to the Furance UI Prefab");
            return;
        }

        consumptionRateText = furnaceUIComponents.consumptionRateText;
        heatProductionText = furnaceUIComponents.heatProductionText;
        furnaceIsOnText = furnaceUIComponents.furnaceIsOnText;
        progressText = furnaceUIComponents.progressText;
        
        inventorySlot = furnaceUI.GetComponentInChildren<InventorySlot>();
        inventorySlot.ItemAdded += ItemAdded;

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
       heatProductionText.text = targetHeatSource.heatPower + "F";
       consumptionRateText.text = consumeItemCountdownTimer.IsRunning ? lastConsumedItemBurnRateInSeconds + "s" : "n/a";
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
        
        consumeItemCountdownTimer.OnTimerStart+= EnableHeatSource; 
        consumeItemCountdownTimer.OnTimerStop += ConsumeItem; 
        consumeItemCountdownTimer.OnTimerStop += DisableHeatSource; 
        lastConsumedItemBurnRateInSeconds = baseFurnaceItemConsumptionRateInSeconds;
    }

    private void EnableHeatSource()
    {
        if (targetHeatSource == null)
            return;
        targetHeatSource.isActive = true;
    }

    private void DisableHeatSource()
    {
        if (targetHeatSource == null)
            return;
        targetHeatSource.isActive = false;
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
        
       lastConsumedItemBurnRateInSeconds = baseFurnaceItemConsumptionRateInSeconds + heldItem.item.burnTimeInSeconds;
        consumeItemCountdownTimer.Reset(lastConsumedItemBurnRateInSeconds);
        consumeItemCountdownTimer.Start();
        
        heldItem.RemoveItem(1);
        
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

       lastConsumedItemBurnRateInSeconds = baseFurnaceItemConsumptionRateInSeconds + heldItem.item.burnTimeInSeconds;
        
        consumeItemCountdownTimer.Reset(lastConsumedItemBurnRateInSeconds);
        consumeItemCountdownTimer.Start();
        
        heldItem.RemoveItem(1);

    }

    void Update()
    {
        consumeItemCountdownTimer.Tick(Time.deltaTime);
        if(furnaceUI != null && furnaceUI.activeSelf)
            PopulateFurnaceUI();
    }
}
