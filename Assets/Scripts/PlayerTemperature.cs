using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerTemperature : MonoBehaviour
{
    // what do we need in this?
    // probably the max distance the player can be from a heat source 
    //public int MaxDistanceFromHeatSourceInMeters = 100;
    //public int CurrentDistanceFromHeatSource = 0;
    public float DamageFromLowTemperature = 5;
    public float DamageTimerLength = 5;
    public float minPlayerTemperatureThreshold = 32.0f; // This field is in Freedom Units
    public float maxPlayerTemperatureThreshold = 150.0f; // This field is in Freedom Units
    [ReadOnly]
    public float currentTemperatureAtPlayer;
    
    private GameObject Player;
    private Health PlayerHealth;
    private Utilities.CountdownTimer DamageTimer;

    private void Awake()
    {
	    Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
        DamageTimer = new Utilities.CountdownTimer(DamageTimerLength);
        DamageTimer.OnTimerStop += PlayerTakeDamage;
    }

    private void PlayerTakeDamage()
    {
        PlayerHealth.TakeDamage(DamageFromLowTemperature);
        DamageTimer.Reset(DamageTimerLength);
        DamageTimer.Start();
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DamageTimer.Tick(Time.deltaTime);

        // need to calculate the heat that the player feels
        currentTemperatureAtPlayer = HeatSourceManagerScript.Instance.GetCurrentTemperature(transform);
        
        if(currentTemperatureAtPlayer >= minPlayerTemperatureThreshold 
           && currentTemperatureAtPlayer <= maxPlayerTemperatureThreshold)
        {
            // if the timer is running then stop it because i'm within warmth
            if (DamageTimer.IsRunning)
                DamageTimer.Pause();
		}
        else 
		{ 
            if(DamageTimer.IsRunning)
            { 
                // do nothing
		    }
            // if the timer is not running then start it
            else
            {
                DamageTimer.Reset(DamageTimerLength);
                DamageTimer.Start();
		    }
		}
    }

}
