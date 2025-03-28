using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;


public class FullScreenTestController : MonoBehaviour
{
    [Header("Refrences")] 
    [SerializeField] private ScriptableRendererFeature fullScreanHeat;
    [SerializeField] private Material material;

    [SerializeField] private Color32 hotColor;
    [SerializeField] private Color32 coldColor; 
    
    [SerializeField] private float _vinIntensity;
    [SerializeField] private float _vorIntensity;
    
    public float belowThreshold = -20f;
    public float aboveThreshold = 20f;
    
    private int _vinIntensityId = Shader.PropertyToID("_Vin_Intensity"); // can be between 0 and 2
    private int _vorIntensityId = Shader.PropertyToID("_Vor_Intensity"); // can be between 0 and 2
    
    private PlayerTemperature _playerTemperature;
    private float _minPlayerTemperature;
    private float _maxPlayerTemperature;
    private float _currentPlayerTemperature;

    private void Awake()
    {
        fullScreanHeat?.SetActive(false);
    }

    private void Start()
    {
        fullScreanHeat.SetActive(false);
        _playerTemperature = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerTemperature>();

        _playerTemperature.OnMinPlayerTemperatureChangeEvent += onMinPlayerChangeEvent;
        _playerTemperature.OnMaxPlayerTemperatureChangeEvent += onMaxPlayerChangeEvent;
        _playerTemperature.OnTemperatureChangedEvent += OnTemperatureChangeEvent;

    }

    private void OnTemperatureChangeEvent(float newTemp) { 
        _currentPlayerTemperature = newTemp;
        CheckTemperature();
    }

    private void CheckTemperature()
    {
        if (_currentPlayerTemperature > _maxPlayerTemperature)
        {
            fullScreanHeat.SetActive(true);
            material.SetColor("_Vin_Color", hotColor);
            var vinIndex = GetIndex(material.shader, _vinIntensityId);
            var minValueVin = ShaderUtil.GetRangeLimits(material.shader, vinIndex, 1);
            var maxValueVin = ShaderUtil.GetRangeLimits(material.shader, vinIndex, 2);
            var percentage = Mathf.InverseLerp(_maxPlayerTemperature, _maxPlayerTemperature + aboveThreshold, _currentPlayerTemperature);
            material.SetFloat(_vinIntensityId, Mathf.Lerp(minValueVin,maxValueVin,percentage));
            
            var vorIndex = GetIndex(material.shader, _vorIntensityId);
            var minValueVor = ShaderUtil.GetRangeLimits(material.shader, vorIndex, 1);
            var maxValueVor = ShaderUtil.GetRangeLimits(material.shader, vorIndex, 2);
            material.SetFloat(_vinIntensityId, Mathf.Lerp(minValueVor,maxValueVor,percentage));
        }
        else if (_currentPlayerTemperature < _minPlayerTemperature)
        {
            fullScreanHeat.SetActive(true);
            material.SetColor("_Vin_Color", coldColor);
            var vinIndex = GetIndex(material.shader, _vinIntensityId);
            var minValueVin = ShaderUtil.GetRangeLimits(material.shader, vinIndex, 1);
            var maxValueVin = ShaderUtil.GetRangeLimits(material.shader, vinIndex, 2);
            var percentage = Mathf.InverseLerp(_minPlayerTemperature, _minPlayerTemperature + belowThreshold, _currentPlayerTemperature);
            material.SetFloat(_vinIntensityId, Mathf.Lerp(minValueVin,maxValueVin,percentage));
            
            var vorIndex = GetIndex(material.shader, _vorIntensityId);
            var minValueVor = ShaderUtil.GetRangeLimits(material.shader, vorIndex, 1);
            var maxValueVor = ShaderUtil.GetRangeLimits(material.shader, vorIndex, 2);
            material.SetFloat(_vinIntensityId, Mathf.Lerp(minValueVor,maxValueVor,percentage));
        }
        else
        {
            fullScreanHeat.SetActive(false);
        }
    }

    private int GetIndex(Shader shader, int propertyID)
    {
        if (shader == null)
            return -1;

        int propertyCount = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < propertyCount; i++)
        {
            string propName = ShaderUtil.GetPropertyName(shader, i);
            int propID = Shader.PropertyToID(propName);

            if (propID == propertyID)
                return i;
        }

        return -1; // Not found
    }

    private void onMinPlayerChangeEvent(float newTemp)
    {
        _minPlayerTemperature = newTemp;
        CheckTemperature();
    }

    private void onMaxPlayerChangeEvent(float newTemp)
    {
        _maxPlayerTemperature = newTemp; 
        CheckTemperature();
    }
}
