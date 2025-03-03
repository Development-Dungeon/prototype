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
            UnlockMouse();
            DeathUi.SetActive(true);
        }
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void UnlockMouse()
    {  
	    FirstPersonController.cameraCanMove = false;
	    Cursor.lockState = CursorLockMode.None;
	    Cursor.visible = true;
    }

    public void LockMouse()
    {  
	    FirstPersonController.cameraCanMove = true;
	    Cursor.lockState = CursorLockMode.Locked;
	    Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
