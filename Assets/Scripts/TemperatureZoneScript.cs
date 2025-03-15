using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureZoneScript : MonoBehaviour
{
    public int baseTemperature;
    
    private void OnTriggerEnter(Collider other)
    {
        // change the base temperature inside the heat source manager
        HeatSourceManagerScript.AddTemperatureZone(this);
    }

    private void OnTriggerExit(Collider other)
    {
        HeatSourceManagerScript.RemoveTemperatureZone(this);
    }

}
