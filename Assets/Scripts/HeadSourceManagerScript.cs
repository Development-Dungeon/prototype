using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniStorm;
using UnityEngine;

// this script is in charge of keeping track of all the heat objects inside the scene
public class HeatSourceManagerScript : MonoBehaviour
{
    public List<HeatSourceScript> heatSources;
    public float baseTemperature = 0.0f;
    public float nightTemperatureNegativeAffect = -10.0f;

    public static HeatSourceManagerScript Instance;
    public List<TemperatureZoneScript> zones;
    
    public UniStormSystem.CurrentTimeOfDayEnum _currentTimeOfDay;

    private void Awake()
    {
        Instance = this;
        heatSources = new(); 
        zones = new();
    }

    void Start()
    {
        UniStormCustomEvents.OnTimeOfDayChanged += OnTimeChange;
    }

    private void OnTimeChange(UniStormSystem.CurrentTimeOfDayEnum currentTimeOfDay)
    {
        _currentTimeOfDay = currentTimeOfDay;
    }

    public void RegisterHeatSource(HeatSourceScript heatSource)
    {
        heatSources ??= new List<HeatSourceScript>();

        heatSources.Add(heatSource);
    }

    public void UnRegisterHeatSource(HeatSourceScript targetHeatSource)
    {
        if (heatSources == null)
            return;

        if(heatSources.Contains(targetHeatSource))
			heatSources.Remove(targetHeatSource);
    }

    public bool TargetWithinDistance(Transform target)
    {
        bool targetWithinDistance = true;
        bool targetNotWithinDistance = false;

        if (target == null || heatSources == null || heatSources.Count == 0)
            return targetNotWithinDistance;

        RemoveNullsFromHeatSource();

        foreach (var hSource in heatSources)
        {
            // get the distance between the heat source and the target
            var distance = Vector3.Distance(hSource.transform.position, target.position);
            // get the power of this heat source
            var power = hSource.heatPower; // this is measured in meters or the same metric as distance
            
            if (power >= distance)
                return targetWithinDistance;
        }

        return targetNotWithinDistance;
    }

    private void RemoveNullsFromHeatSource()
    {
        if (heatSources == null || heatSources.Count == 0)
            return;

        heatSources.RemoveAll(source => source == null || source.gameObject == null);
    }

    private float getBaseTemperature()
    {
        var result = baseTemperature;
        
        if (Instance.zones.Count > 0)
            result = Instance.zones.Last().baseTemperature;
        
        if(_currentTimeOfDay == UniStormSystem.CurrentTimeOfDayEnum.Night)
            result -= nightTemperatureNegativeAffect;
        
        return result;
    }

    public float GetCurrentTemperature(Transform target)
    {
        if (target is null || heatSources is null || heatSources.Count == 0)
            return baseTemperature;

        RemoveNullsFromHeatSource();

        var maxTemperature = getBaseTemperature();
        
        foreach (var hSource in heatSources)
        {
            if (!hSource.isActive)
                continue;
            // get the distance between the heat source and the target
            var distance = Vector3.Distance(hSource.transform.position, target.position);
            // get the power of this heat source
            var power = hSource.heatPower; // this is measured in meters or the same metric as distance
            var heatDissipationRate = hSource.dissipationRateByDistance;
            
            var heatAtPlayerLocation =  getBaseTemperature() + (power - (distance * heatDissipationRate));

                
            
            maxTemperature = Mathf.Max(maxTemperature, heatAtPlayerLocation);
        }
        
        var roundedTemperature = Mathf.RoundToInt(maxTemperature);

        return roundedTemperature;

    }

    public static void AddTemperatureZone(TemperatureZoneScript temperatureZoneScript)
    {
        if (Instance.zones.Contains(temperatureZoneScript))
        {
            Instance.zones.Remove(temperatureZoneScript);
        }
        
        Instance.zones.Add(temperatureZoneScript);
        
    }

    public static void RemoveTemperatureZone(TemperatureZoneScript temperatureZoneScript)
    {
        Instance.zones.Remove(temperatureZoneScript);
    }
}
