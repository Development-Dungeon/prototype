using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class FullScreenTestController : MonoBehaviour
{
    [Header("Refrences")] [SerializeField] private ScriptableRendererFeature fullScreanHeat;
    
    private void Start()
    {
        fullScreanHeat.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fullScreanHeat.SetActive(true);
        }
        
    }
}
