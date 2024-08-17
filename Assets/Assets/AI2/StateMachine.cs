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
        CurrentState?.EnterState(gameObject);
    }

    private void Update()
    {
        EState nextStateKey = CurrentState.GetNextState(gameObject);

        if (nextStateKey.Equals(CurrentState.StateKey))
            CurrentState?.UpdateState(gameObject);
        else
            TransitionToState(nextStateKey);
    }


    private void TransitionToState(EState stateKey)
    {
        CurrentState.ExistState(gameObject);
        CurrentState = States[stateKey];
        CurrentState.EnterState(gameObject);

        stateTransition?.Invoke(gameObject, stateKey);

    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState?.OnTriggerEnter(gameObject, other);
    }

    private void OnTriggerStay(Collider other)
    {
        CurrentState?.OnTriggerStay(gameObject, other);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentState?.OnTriggerExit(gameObject, other);
    }
}
