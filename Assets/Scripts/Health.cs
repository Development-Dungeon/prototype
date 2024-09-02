using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This class is used for any object which may have health
public class Health : MonoBehaviour
{

    public float _maxHealth = 100;
    public float _currentHealth;
    public event Action<float> PlayerHealthPercentEvent;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        _currentHealth = currentHealth;
    }

    void Start()
    {
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0) _currentHealth = 0;

        PlayerHealthPercentEvent?.Invoke(_currentHealth / _maxHealth);
    }


    public bool IsDead() => _currentHealth <= 0;

}
