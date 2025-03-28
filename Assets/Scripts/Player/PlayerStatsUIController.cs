using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerStatsUIController : MonoBehaviour
{

    public TMP_Text oxygenText;
    public TMP_Text depthText;
    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text playerReachText;
    public TMP_Text attackRangeText;
    public TMP_Text attackCooldownText;
    public TMP_Text attackAoeText;
    public TMP_Text playerTemperatureText;
    public TMP_Text playerMinTemperatureText;
    public TMP_Text playerMaxTemperatureText;

    public GameObject ui;

    private GameObject player;
    private PlayerDepth playerDepth;
    private Health playerHealth;
    private Oxygen playerOxygen;
    private PlayerReach playerReach;
    private PlayerTemperature playerTemperature;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        playerHealth = player.GetComponent<Health>(); 
	    playerHealth.HealthUpdatedEvent += HealthUpdatedEvent;

        playerOxygen = player.GetComponent<Oxygen>();
		playerOxygen.OxygenUpdateEvent += OxygenUpdateEvent;

        playerDepth = player.GetComponentInChildren<PlayerDepth>();
	    playerDepth.PlayerDepthUpdateEvent += DepthUpdateEvent ;

        playerReach = player.GetComponentInChildren<PlayerReach>();
	    playerReach.ReachUpdateEvent += ReachUpdateEvent;
        
        playerTemperature = player.GetComponentInChildren<PlayerTemperature>();
        playerTemperature.OnTemperatureChangedEvent += PlayerOnTemperatureChangedEvent;

        InventoryManagerNew.SelectedItemChanged += SelectedItemChangedEvent;


        // initialize those states which don't change on startup or can't be relied on based on the startup order
        ReachUpdateEvent(playerReach);
        HealthUpdatedEvent(playerHealth);
        OxygenUpdateEvent(playerOxygen);
        PlayerOnTemperatureChangedEvent(playerTemperature.currentTemperatureAtPlayer);
    }

    private void PlayerOnTemperatureChangedEvent(float currentPlayerTemperature)
    {
        if(playerTemperatureText != null)
            playerTemperatureText.text = $"P. Cur Temp : {currentPlayerTemperature}F";
        else 
            playerTemperatureText.text = $"P. Cur Temp : n/a";
        
        if(playerMinTemperatureText != null)
            playerMinTemperatureText.text = $"P. Min Temp : {playerTemperature.minPlayerTemperatureThreshold}F";
        else 
            playerMinTemperatureText.text = $"P. Min Temp : n/a";
        
        if(playerMaxTemperatureText != null)
            playerMaxTemperatureText.text = $"P. Max Temp: {playerTemperature.maxPlayerTemperatureThreshold}F";
        else 
            playerMaxTemperatureText.text = $"P. Max Temp : n/a";
    }

    public void Update()
    {
        if(Input.inputString != null) {
            if (Input.GetKeyDown(KeyCode.P)){
                ToggleUI();
		    }
        }
    }

    private void ToggleUI()
    {
        ui.SetActive(!ui.activeSelf);
    }

    private void ReachUpdateEvent(PlayerReach playerReachChangeInfo)
    {
        if (playerReachText == null)
            return;

        if (playerReachChangeInfo == null)
			playerReachText.text = $"Reach : 0";
        else
			playerReachText.text = $"Reach : {playerReachChangeInfo.reachDistance}";

    }

    private void SelectedItemChangedEvent(Item selectedItemChangeInfo)
    {

        if (selectedItemChangeInfo == null || !selectedItemChangeInfo.actionType.Equals(ActionType.Attack))
        { 
            if(attackText != null)
			    attackText.text = $"Attack : n/a";
            if(attackRangeText != null)
			    attackRangeText.text = $"Attack Range : n/a";
            if(attackCooldownText != null)
			    attackCooldownText.text = $"Attack cooldown : n/a";
            if(attackAoeText != null)
			    attackAoeText.text = $"Attacks multiple : n/a";
		}
        else
        {  
            if(attackText != null)
			    attackText.text = $"Attack : {selectedItemChangeInfo.damage}";

            if(attackRangeText != null)
			    attackRangeText.text = $"Attack Range : {selectedItemChangeInfo.range}";
            
            if(attackCooldownText != null)
			    attackCooldownText.text = $"Attack cooldown : {selectedItemChangeInfo.cooldownInSeconds}";

            if(attackAoeText != null)
			    attackAoeText.text = $"Attacks multiple : {selectedItemChangeInfo.canAttackMultiple}";
        }

    }


    private void DepthUpdateEvent(PlayerDepth depthEventInfo)
    {
        if (depthEventInfo == null)
            return;
        if (depthText == null)
            return;

        depthText.text = $"Depth : {depthEventInfo._currentDepth} / {depthEventInfo._maxDepth}";

    }

    private void OxygenUpdateEvent(Oxygen oxygenEventInfo)
    {

        if (oxygenEventInfo == null)
            return;
        if (oxygenText == null)
            return;

        var current = Mathf.RoundToInt(oxygenEventInfo._current);

        oxygenText.text = $"Oxygen : {current} / {oxygenEventInfo._max}";

    }

    private void HealthUpdatedEvent(Health healthEventInfo)
    {
        if (healthEventInfo == null)
            return;
        if (healthText == null)
            return;

        healthText.text = $"Health : {healthEventInfo._currentHealth} / {healthEventInfo._maxHealth}";

    }

    private void OnDestroy()
    {
        playerHealth.HealthUpdatedEvent -= HealthUpdatedEvent;
        playerOxygen.OxygenUpdateEvent -= OxygenUpdateEvent;
        playerDepth.PlayerDepthUpdateEvent -= DepthUpdateEvent;
        playerReach.ReachUpdateEvent -= ReachUpdateEvent;

        InventoryManagerNew.SelectedItemChanged -= SelectedItemChangedEvent;
    }

}
