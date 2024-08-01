using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    private float _maxOxygen = 100;
    public float _currentOxygen;
    public Image _oxygenBarFill;


    void Start()
    {
        _currentOxygen = _maxOxygen;
    }
    public void UpdateOxygen(float amount)
    {
        _currentOxygen += amount;
        UpdateOxygenBar();
    }

    private void UpdateOxygenBar()
    {

        float targetFillAmount = _currentOxygen / _maxOxygen;
        _oxygenBarFill.fillAmount = targetFillAmount;


    }
    private void Update()
    {
        _oxygenBarFill.fillAmount = _currentOxygen / _maxOxygen;



    }
}
