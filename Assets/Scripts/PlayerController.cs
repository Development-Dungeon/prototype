using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Health")]
    public GameObject healthBar;
    private Health health;


    private void Awake()
    {
        health = GetComponent<Health>();
        health.PlayerHealthPercentEvent += UpdateHealthBarPercent;
    }

    private void UpdateHealthBarPercent(float percent)
    {
        healthBar.GetComponent<HealthBar>()?.UpdateFillAmount(percent);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
