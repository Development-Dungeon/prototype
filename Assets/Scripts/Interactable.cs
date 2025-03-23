using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    public bool showHelpText = true;
    public string name; // This is only for the inspector so that the action can be named
    public KeyCode interactKey;
    public UnityEvent OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
    }
    
    public void ToggleShowHelpText() { showHelpText = !showHelpText; }

}
