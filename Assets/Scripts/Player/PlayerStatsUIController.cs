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

    public GameObject player;
    public GameObject ui;

    private PlayerDepth playerDepth;
    private Health playerHealth;
    private Oxygen playerOxygen;
    private PlayerReach playerReach;


    void Start()
    {
        playerHealth = player.GetComponent<Health>(); 
	    playerHealth.HealthUpdatedEvent += HealthUpdatedEvent;

        playerOxygen = player.GetComponent<Oxygen>();
		playerOxygen.OxygenUpdateEvent += OxygenUpdateEvent;

        playerDepth = player.GetComponentInChildren<PlayerDepth>();
	    playerDepth.PlayerDepthUpdateEvent += DepthUpdateEvent ;

        playerReach = player.GetComponentInChildren<PlayerReach>();
	    playerReach.ReachUpdateEvent += ReachUpdateEvent;

        InventoryManagerNew.SelectedItemChanged += SelectedItemChangedEvent;


        // initialize those states which don't change on startup or can't be relied on based on the startup order
        ReachUpdateEvent(playerReach);
        HealthUpdatedEvent(playerHealth);
        OxygenUpdateEvent(playerOxygen);
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
