using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    public Image _healthBarFill;
    Health health;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<Health>();
        health.HealthPercentChangeEvent += UpdateFillAmount;
    }

    public void UpdateFillAmount(float percent) => _healthBarFill.fillAmount = percent;

    private void OnDestroy() => health.HealthPercentChangeEvent -= UpdateFillAmount;
}
