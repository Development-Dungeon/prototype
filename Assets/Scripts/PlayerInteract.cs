using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask interactLayer;
    public GameObject interactTextParent;
    
    private TMP_Text _interactText;
    private GameObject _player;
    private PlayerReach _playerReach;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerReach = _player.GetComponentInChildren<PlayerReach>();
        _interactText = interactTextParent.GetComponentInChildren<TMP_Text>();
    }
    
    private void Update()
    {
        
        var ray = new Ray(_playerReach.transform.position, _playerReach.transform.forward);

        if (!Physics.Raycast(ray, out var hit, _playerReach.reachDistance, interactLayer)) {
            interactTextParent.SetActive(false);
            return;
        }
        
        var interactable = hit.transform.GetComponent<Interactable>();

        if (!interactable)
        {
            interactTextParent.SetActive(false);
            return;
        }

        _interactText.text = $"Click {interactable.interactKey.ToString()}";
        interactTextParent.SetActive(interactable.showHelpText);
        
        if (Input.GetKeyDown(interactable.interactKey))
        {
            interactable.Interact();
            interactTextParent.SetActive(false);
        }
    }
}
