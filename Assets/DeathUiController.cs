using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUiController : MonoBehaviour
{
    public GameObject DeathUi;
    public GameObject Player;
    void Start()
    {
        Player.GetComponent<Health>().HealthPercentChangeEvent += HealthUpdateEvent;
    }

    private void HealthUpdateEvent(float health)
    {
      
        if (health == 0)
        {
            DeathUi.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
