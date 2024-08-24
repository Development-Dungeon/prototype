using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpdateStateText : MonoBehaviour
{

    void Awake()
    {

        StateMachine.StateMachineNewStateEvent += UpdateTextForStateMachine;
    }

    private void UpdateTextForStateMachine(Type newState)
    {

        var textGO = transform.Find("canvasGO")?.GetComponent<TextMeshPro>();

        textGO.text = newState.Name;

    }

}
