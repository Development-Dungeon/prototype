using System;
using System.Collections;
using System.Collections.Generic;
using UniStorm;
using UnityEngine;

// The purupose of this script is to create custom events which are not exposed via unistorm.
// These events can be used by any system which requires it
public class UniStormCustomEvents : MonoBehaviour
{
    
    public static Action<UniStormSystem.CurrentTimeOfDayEnum> OnTimeOfDayChanged;
    
    private UniStormSystem.CurrentTimeOfDayEnum _currentTimeOfDay;
    

    void Update()
    {
        if (_currentTimeOfDay == UniStormSystem.Instance.CurrentTimeOfDay) return;
        
        _currentTimeOfDay = UniStormSystem.Instance.CurrentTimeOfDay;
        OnTimeOfDayChanged?.Invoke(_currentTimeOfDay);

    }
}
