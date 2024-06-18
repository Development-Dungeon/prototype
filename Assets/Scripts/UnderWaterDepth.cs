using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderWaterDepth : MonoBehaviour
{
    [Header("Depth Parameters")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private int depth = 6;

    [Header("Post Processing Volume")]
    [SerializeField] private Volume postProcessingVolume;

    [Header("Post Processing Profiles")]
    [SerializeField] private VolumeProfile surfacePostProcessing;
    [SerializeField] private VolumeProfile underwaterPostProcessing;

    [Header("Fog Type")]
    [SerializeField] private Color fogColor;

    [Header("Fog Density")]
    [SerializeField] private float fogStartDensity;
    [SerializeField] private float fogEndDensity;


    private void Update()
    {
        if (mainCamera.position.y < depth)
        {
            
            EnableEffects(true);
        }
        else
        {
            EnableEffects(false);
        }
    }

    private void EnableEffects(bool active)
    {
        if (active)
        {
            
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = fogStartDensity;
            RenderSettings.fogEndDistance = fogEndDensity;
            RenderSettings.fogColor = fogColor;
            postProcessingVolume.profile = underwaterPostProcessing;
            
        }
        else 
        {
            RenderSettings.fog = false;
            postProcessingVolume.profile = surfacePostProcessing;
           
        }
    
    }

}
