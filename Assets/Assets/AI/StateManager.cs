using UnityEngine;
using System.Collections;
using System;

public class StateManager : MonoBehaviour
{

    public event Action<State> StatusChangeEvent;
    public State currentState;
    public EnemyAttributes enemyAttributes;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(this);

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(State nextState)
    {
        if (nextState.GetType() != currentState.GetType())
            StatusChangeEvent?.Invoke(nextState);

        currentState = nextState;
    }


}
