using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    public GameObject Entity;
    public Image _healthBarFill;
    Health health;

    private void Awake()
    {
        health = Entity.GetComponentInChildren<Health>();
        health.HealthPercentChangeEvent += UpdateFillAmount;
    }

    public void UpdateFillAmount(float percent) => _healthBarFill.fillAmount = percent;

    private void OnDestroy() => health.HealthPercentChangeEvent -= UpdateFillAmount;
}
