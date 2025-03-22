using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public KeyCode interactKey;
    public UnityEvent OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
    }

}
