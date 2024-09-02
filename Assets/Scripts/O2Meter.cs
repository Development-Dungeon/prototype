using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O2MeterBar : MonoBehaviour
{
    public Image _o2Meter;

    private void Awake()
    {
        Oxygen.OxygenPercentChangeEvent += UpdateFillAmount;
    }

    public void UpdateFillAmount(float percent)
    {
        _o2Meter.fillAmount = percent;
    }

    private void OnDestroy()
    {
        Oxygen.OxygenPercentChangeEvent -= UpdateFillAmount;
    }
}
