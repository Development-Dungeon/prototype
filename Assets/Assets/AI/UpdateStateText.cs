using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateStateText : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {

        // find the state manager on the object
        var stateManager = this.GetComponent<StateManager>();

        // subscribe to event
        stateManager.StatusChangeEvent += UpdateTextForStateChange;

        
    }

    public void UpdateTextForStateChange(State newState)
    {
        // find the canvasGO on the object
        var textGO = transform.Find("canvasGO")?.GetComponent<TextMeshPro>();

        textGO.text = newState.GetType().Name;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
