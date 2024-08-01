using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBar : MonoBehaviour
{
    private float _maxHealth = 100;
    public float _currentHealth;
    public Image _healthBarFill;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void UpdateHealth(float amount)
    {
        _currentHealth += amount;
        UpdateHealthBar();

    }
    private void UpdateHealthBar()
    {

        float targetFillAmount = _currentHealth / _maxHealth;
        _healthBarFill.fillAmount = targetFillAmount;

       
    }
    private void Update()
    {
        _healthBarFill.fillAmount = _currentHealth / _maxHealth;
        
    }

    
    

}

