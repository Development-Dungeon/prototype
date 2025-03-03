using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManger : MonoBehaviour
{

    public GameObject playerGo;
    public Transform respawnLocation; 
    private Health playerHealth;
    private FirstPersonController playerController;


    void Start()
    {
        playerHealth = playerGo.GetComponent<Health>();
    }
    public void Respawn()
    {
        if (respawnLocation != null)
        {
            playerHealth.SetCurrentHealth(playerHealth._maxHealth);
            playerGo.transform.position = respawnLocation.position;
            playerGo.transform.rotation = respawnLocation.rotation;
        }
    }
}
