using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{

    public event Action<GameObject, EState> stateTransition;

    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    private void Start()
    {
        CurrentState?.EnterState(this);
    }

    private void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
            CurrentState?.UpdateState();
        else
            TransitionToState(nextStateKey);
    }


    private void TransitionToState(EState stateKey)
    {
        CurrentState.ExistState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();

        stateTransition?.Invoke(gameObject, stateKey);

    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState?.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CurrentState?.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentState?.OnTriggerExit(other);
    }
}
