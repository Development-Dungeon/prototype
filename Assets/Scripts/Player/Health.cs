using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This class is used for any object which may have health
public class Health : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;
    public event Action<float> HealthPercentChangeEvent;
    public event Action<Health> HealthUpdatedEvent;

    private void Awake() => _currentHealth = _maxHealth;
    private void Start() => TriggerEvent();
    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;

        if (_maxHealth <= 0) _maxHealth = 0;
        if (_currentHealth >= _maxHealth) _currentHealth = _maxHealth;
        if (_currentHealth <= 0) _currentHealth = 0;

        TriggerEvent();
    }

    public void SetCurrentHealth(float currentHealth)
    {
        _currentHealth = currentHealth;

        if (_currentHealth >= _maxHealth) _currentHealth = _maxHealth;
        if (_currentHealth <= 0) _currentHealth = 0;

        TriggerEvent();
    }

    public void IncreaseCurrentHealth(float amount)
    {
        SetCurrentHealth(amount + _currentHealth);
    }

    public bool IsDead() => _currentHealth <= 0;

    private void TriggerEvent()
    {  
        if(_maxHealth != 0 && HealthPercentChangeEvent != null)
			HealthPercentChangeEvent.Invoke(_currentHealth / _maxHealth);
        if (HealthUpdatedEvent != null)
            HealthUpdatedEvent.Invoke(this);
    }

    public void IncreaseCurrentAndMax(float additinalCurrentHealth, float additionalMax )
    {
        SetMaxHealth(_maxHealth + additionalMax);
        SetCurrentHealth(_currentHealth + additinalCurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0) _currentHealth = 0;

        TriggerEvent();
    }



}
