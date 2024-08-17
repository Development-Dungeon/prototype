using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdateStateText : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {

        // find the state manager on the object
        var stateManager = this.GetComponent<StateManager>();

        // subscribe to event
        stateManager.StatusChangeEvent += UpdateTextForStateChange;
        var statemachineV2 = this.GetComponent<FishStateMachine>();

        statemachineV2.stateTransition += UpdateTextForStateChange;

        
    }

    public void UpdateTextForStateChange(State newState)
    {
        // find the canvasGO on the object
        var textGO = transform.Find("canvasGO")?.GetComponent<TextMeshPro>();

        textGO.text = newState.GetType().Name;

    }

    public void UpdateTextForStateChange<StateEnum> (GameObject go, StateEnum newstate ) where StateEnum : Enum
    {  
        var textGO = transform.Find("canvasGO")?.GetComponent<TextMeshPro>();

        textGO.text = newstate.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
