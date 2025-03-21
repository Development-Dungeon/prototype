using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// in this class on fixed update i need to do a raycast on a certain layer to see if i hit anything on that layer
public class PlayerInteract : MonoBehaviour
{
    
    public LayerMask interactLayer;
    public GameObject interactTextParent;
    
    private TMP_Text interactText;
    private GameObject player;
    private PlayerReach playerReach;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerReach = player.GetComponentInChildren<PlayerReach>();
        interactText = interactTextParent.GetComponentInChildren<TMP_Text>();
    }

    private void FixedUpdate()
    {
        
        var playerRay = new Ray(player.transform.position, player.transform.forward);
        Debug.DrawLine(playerRay.origin, playerRay.origin + playerRay.direction * playerReach.reachDistance, Color.magenta);
        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var ray = new Ray(playerReach.transform.position, playerReach.transform.forward);

        if (!Physics.Raycast(ray, out var hit, playerReach.reachDistance, interactLayer))
        {
            interactTextParent.SetActive(false);
            return;
        }
        
        var interactable = hit.transform.GetComponent<Interactable>();

        if (!interactable)
        {
            interactTextParent.SetActive(false);
            return;
        }

        if (!interactTextParent.activeSelf)
            interactTextParent.SetActive(true);
        
        interactText.text = $"Click {interactable.interactKey.ToString()}";
            
        if (Input.GetKeyDown(interactable.interactKey))
        {
            interactable.Interact();
            interactTextParent.SetActive(false);
        }
    }
}
