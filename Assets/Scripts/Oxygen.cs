using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Oxygen : MonoBehaviour
{

    public float _max = 100;
    public float _usePerSecond;
    public float _restorePerSecond;
    public float _current;
    public bool on;
    public event Action<float> OxygenPercentChangeEvent;


    void Awake() => _current = _max;
    public void On() => on = true;
    public void Off() => on = false;
    public void UseOxygen(float amountUsed) => SetCurrent(_current - amountUsed);
    public void AddOxygen(float amountToAdd) => SetCurrent(_current + amountToAdd);


    public void SetMax(float max) 
    {
		_max = max;
        TriggerEvent();
	}

    public void SetCurrent(float current)
    {
        _current = current;
        if (_current < 0) _current = 0;
        if (_current > _max) _current = _max;
        TriggerEvent();
    }

    private void TriggerEvent()
    {
        if(_max != 0 && OxygenPercentChangeEvent != null) 
			OxygenPercentChangeEvent.Invoke(_current / _max);

    }

    private void Update()
    {
        if (on)
            UseOxygen(_usePerSecond * Time.deltaTime);
        else
            AddOxygen(_restorePerSecond * Time.deltaTime);
    }

}
