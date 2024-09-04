using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class O2MeterBar : MonoBehaviour
{
    public Image _o2Meter;
    Oxygen oxygen;

    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        oxygen = player.GetComponent<Oxygen>();
        oxygen.OxygenPercentChangeEvent += UpdateFillAmount;
    }

    public void UpdateFillAmount(float percent) => _o2Meter.fillAmount = percent;
    private void OnDestroy() => oxygen.OxygenPercentChangeEvent -= UpdateFillAmount;
}
