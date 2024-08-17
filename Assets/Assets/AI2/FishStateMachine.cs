using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateMachine : StateMachine<FishStateMachine.FishState>
{
    public enum FishState {  
        Idle, 
        Wander, 
        Chase,
        Attack
    }

    private void Awake()
    {
        States.Add(FishState.Idle, new FishIdelState() );
        States.Add(FishState.Wander, new FishWanderState() );
        States.Add(FishState.Chase, new FishChaseState() );
        States.Add(FishState.Attack, new FishAttackState() );

        CurrentState = States[FishState.Idle];
    }


}
