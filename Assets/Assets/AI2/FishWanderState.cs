using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWanderState : BaseState<FishStateMachine.FishState> 
{

    private bool wanderComplete;

    public FishWanderState() : base(FishStateMachine.FishState.Wander)
    {  }

    public override void EnterState()
    {
    }

    public override void ExistState()
    {
    }

    public override void UpdateState()
    {
        // find a location to travel to
        // move and look at that location
    }

    public override FishStateMachine.FishState GetNextState()
    {
        if (wanderComplete)
            return FishStateMachine.FishState.Idle;
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    public override void OnTriggerStay(Collider other)
    {
    }

}
