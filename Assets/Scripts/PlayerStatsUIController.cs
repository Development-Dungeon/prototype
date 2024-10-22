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
    public GameObject player;


    void Start()
    {
        player.GetComponent<Health>().HealthUpdatedEvent += HealthUpdatedEvent;
        player.GetComponent<Oxygen>().OxygenUpdateEvent += OxygenUpdateEvent;
        player.GetComponentInChildren<PlayerDepth>().PlayerDepthUpdateEvent += DepthUpdateEvent ;
        player.GetComponentInChildren<PlayerReach>().ReachUpdateEvent += ReachUpdateEvent;
        InventoryManagerNew.SelectedItemChanged += SelectedItemChangedEvent;


        // initialize those states which don't change on startup or can't be relied on based on the startup order
        ReachUpdateEvent(player.GetComponentInChildren<PlayerReach>());
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
        if (attackText == null)
            return;

        else if (selectedItemChangeInfo == null)
		    attackText.text = $"Attack : 0";
        else if (!selectedItemChangeInfo.actionType.Equals(ActionType.Attack))
		    attackText.text = $"Attack : 0";
        else 
		    attackText.text = $"Attack : {selectedItemChangeInfo.damage}";

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
        player.GetComponent<Health>().HealthUpdatedEvent -= HealthUpdatedEvent;
        player.GetComponent<Oxygen>().OxygenUpdateEvent -= OxygenUpdateEvent;
        player.GetComponent<PlayerDepth>().PlayerDepthUpdateEvent -= DepthUpdateEvent;
        player.GetComponentInChildren<PlayerReach>().ReachUpdateEvent -= ReachUpdateEvent;
        InventoryManagerNew.SelectedItemChanged -= SelectedItemChangedEvent;
    }

}
