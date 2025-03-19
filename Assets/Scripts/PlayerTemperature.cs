using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class PlayerTemperature : MonoBehaviour
{
    [Header("Player Damage Settings")] 
    public bool enablePlayerDamage = true;
    public float damageFromLowTemperature = 5;
    public float damageTimerLength = 5;
    public float minPlayerTemperatureThreshold = 32.0f; 
    public float maxPlayerTemperatureThreshold = 150.0f; 
    public float currentTemperatureAtPlayer;
    
    [Header("Player Healing Settings")]
    public bool enablePlayerHealing = true;
    public float minPlayerTemperatureThresholdForHealing = 80.0f;
    public float maxPlayerTemperatureThresholdForHealing = 90.0f;
    public float healingTimerPeriodInSeconds = 10.0f;
    public float healingFromTemperature = 1.0f;
    private CountdownTimer _healingTimer;
    
    public event Action<float> TemperatureChangedEvent;
    
    private GameObject _player;
    private Health _playerHealth;
    private CountdownTimer _damageTimer;

    private void Awake()
    {
	    _player = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _player.GetComponent<Health>();
        _damageTimer = new CountdownTimer(damageTimerLength);
        _damageTimer.OnTimerStop += PlayerTakeDamage;
        
        _healingTimer = new CountdownTimer(healingTimerPeriodInSeconds);
        _healingTimer.OnTimerStop += HealPlayer;
    }

    private void CalculateHealingTemperatureEffect()
    {
        if (!enablePlayerHealing)
        {
            if(_healingTimer.IsRunning)
                _healingTimer.Pause();
            return;
        }
        
        var playerWithinHealingTemperatureRange = currentTemperatureAtPlayer >= minPlayerTemperatureThresholdForHealing && currentTemperatureAtPlayer <= maxPlayerTemperatureThresholdForHealing;
        
        if (_healingTimer.IsRunning && playerWithinHealingTemperatureRange) return;
        
        if (_healingTimer.IsRunning && !playerWithinHealingTemperatureRange)
        {
            _healingTimer.Pause();
        }
        else if (!_healingTimer.IsRunning && playerWithinHealingTemperatureRange)
        {
            _healingTimer.Reset(healingTimerPeriodInSeconds);
            _healingTimer.Start();
        }

    }

    private void HealPlayer()
    {
        _playerHealth.IncreaseCurrentHealth(healingFromTemperature);
        _healingTimer.Reset(healingTimerPeriodInSeconds);
        _healingTimer.Start();
    }

    private void PlayerTakeDamage()
    {
        _playerHealth.TakeDamage(damageFromLowTemperature);
        _damageTimer.Reset(damageTimerLength);
        _damageTimer.Start();
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _damageTimer.Tick(Time.deltaTime);
        _healingTimer.Tick(Time.deltaTime);

        // need to calculate the heat that the player feels
        SetCurrentTemperatureAtPlayer(HeatSourceManagerScript.Instance.GetCurrentTemperature(transform));
        
        CalculateColdTemperatureEffect();
        CalculateHealingTemperatureEffect();
    }


    private void CalculateColdTemperatureEffect()
    {

        if (!enablePlayerDamage)
        {
            if(_damageTimer.IsRunning)
                _damageTimer.Pause();
            return;
        }
        
        if(currentTemperatureAtPlayer >= minPlayerTemperatureThreshold 
           && currentTemperatureAtPlayer <= maxPlayerTemperatureThreshold)
        {
            // if the timer is running then stop it because i'm within warmth
            if (_damageTimer.IsRunning)
                _damageTimer.Pause();
        }
        else 
        { 
            if(_damageTimer.IsRunning)
            { 
                // do nothing
            }
            // if the timer is not running then start it
            else
            {
                _damageTimer.Reset(damageTimerLength);
                _damageTimer.Start();
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
