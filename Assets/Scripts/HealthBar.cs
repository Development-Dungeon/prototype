using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    private float _maxHealth = 100;
    private float _currentHealth;
    public Image _healthBarFill;

    public static event Action<float> UpdateHealthEvent;

    void Start()
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
        UpdateHealthBar();
    }

    public void UpdateHealth(float amount)
    {
        _currentHealth += amount;
        UpdateHealthBar();

    }

    public void UpdateHealthEven(float healthToUpdate)
    {
        if (_currentHealth <= 0) return;
        UpdateHealth(healthToUpdate);
        if (_currentHealth < 0) _currentHealth = 0;
    }

    private void UpdateHealthBar()
    {

        float targetFillAmount = _currentHealth / _maxHealth;

        if (targetFillAmount <= 0)
            targetFillAmount = 0;

        _healthBarFill.fillAmount = targetFillAmount;
    }

    public void UpdateFillAmount(float percent)
    {
        _healthBarFill.fillAmount = percent;
    }
}
