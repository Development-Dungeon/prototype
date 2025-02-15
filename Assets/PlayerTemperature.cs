using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTemperature : MonoBehaviour
{
    // what do we need in this?
    // probably the max distance the player can be from a heat source 
    //public int MaxDistanceFromHeatSourceInMeters = 100;
    //public int CurrentDistanceFromHeatSource = 0;
    public float DamageFromLowTemperature = 5;
    public float DamageTimerLength = 5;
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
    void Update()
    {
        DamageTimer.Tick(Time.deltaTime);

        if (HeatSourceManagerScript.Instance.TargetWithinDistance(transform))
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
                DamageTimer.Reset(DamageFromLowTemperature);
                DamageTimer.Start();
		    }
		}
    }
}
