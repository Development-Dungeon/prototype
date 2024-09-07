using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Oxygen : MonoBehaviour
{

    public readonly float _defaultMax = 100;
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
        if (_current > _max) _current = _max;
        TriggerEvent();
	}

    public void SetCurrent(float current)
    {
        // if i am already at 0 and the new current is less, then return and do nothing. do not send another event
        if (_current == 0 && current <= 0) return;

        // if i am already maxed and the new current will put me further, return and do nothing
        if (_current >= _max && current >= _max) return;

        // else, the new current is adding o2
        // else, the new current is substracting o2
        // in either case there is going to be a change in 02

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
