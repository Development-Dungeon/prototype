using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class HeatSourceScript : MonoBehaviour
{

    [Header("General Settings")]
    public float heatPower = 10.0f;
    public float dissipationRateByDistance = 1.0f;
    public bool isActive = false;
    
    [Header("Temporary Heat Sources Only")]
    public bool isDepletableHeatSource = false;
    public float heatPeriodInSeconds= 1.0f;
    public float extingushRate = 1.0f;
    private CountdownTimer heatTimer;
    
    [Header("Debug Settings")]
    public bool drawGizmos = false;
    
    private void Awake()
    {
        InitHeatTimer();
    }

    private void InitHeatTimer()
    {
        if (!isDepletableHeatSource) return;
        
        if (heatTimer == null)
        {
            heatTimer = new CountdownTimer(heatPeriodInSeconds);
            heatTimer.OnTimerStop += DepleteHeatSource;
            heatTimer.Start();
        }
        else if (!heatTimer.IsRunning)
        {
            heatTimer.Reset();
            heatTimer.Start();
        }
        
    }

    private void DepleteHeatSource()
    {
        if (!isDepletableHeatSource) return;
        if (!isActive) return;
        
        if(heatPower > 0.0f)
            heatPower -= extingushRate;

        if (heatPower <= 0.0f)
        {
            isDepletableHeatSource = false;
            isActive = false;
            return;
        }
        
        heatTimer.Reset();
        heatTimer.Start();
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        if (dissipationRateByDistance == 0.0f) return;
        
        Gizmos.color = Color.red;

        var radius = heatPower / dissipationRateByDistance;
        
        Gizmos.DrawWireSphere(transform.position, radius); 
    }

    private void FixedUpdate()
    {
        if (isDepletableHeatSource)
        {
            InitHeatTimer();
            
            heatTimer.Tick(Time.deltaTime);
        }
    }

    private void Start()
    {
        if(HeatSourceManagerScript.Instance != null)
			HeatSourceManagerScript.Instance.RegisterHeatSource(this);
    }


    private void OnDestroy()
    {
        if(HeatSourceManagerScript.Instance != null)
			HeatSourceManagerScript.Instance.UnRegisterHeatSource(this);
    }

}
