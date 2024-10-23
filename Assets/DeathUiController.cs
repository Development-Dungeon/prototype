using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
