using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    public Image _healthBarFill;

    private void Awake()
    {
        Health.HealthPercentChangeEvent += UpdateFillAmount;
    }

    public void UpdateFillAmount(float percent)
    {
        _healthBarFill.fillAmount = percent;
    }

    private void OnDestroy()
    {
        Health.HealthPercentChangeEvent -= UpdateFillAmount;
    }
}
