using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerTemperature : MonoBehaviour
{
    public float DamageFromLowTemperature = 5;
    public float DamageTimerLength = 5;
    public float minPlayerTemperatureThreshold = 32.0f; 
    public float maxPlayerTemperatureThreshold = 150.0f; 
    public float currentTemperatureAtPlayer;
    
    public event Action<float> TemperatureChangedEvent;
    
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
        SetCurrentTemperatureAtPlayer(HeatSourceManagerScript.Instance.GetCurrentTemperature(transform));
        
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

    private void SetCurrentTemperatureAtPlayer(float newTemperature)
    {
        if (Mathf.Approximately(newTemperature, currentTemperatureAtPlayer))
            return;
        
        currentTemperatureAtPlayer = newTemperature;
        
        if(TemperatureChangedEvent != null)
            TemperatureChangedEvent.Invoke(currentTemperatureAtPlayer);
        
    }
}
