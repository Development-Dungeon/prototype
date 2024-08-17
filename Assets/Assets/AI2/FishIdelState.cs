using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishIdelState : BaseState<FishStateMachine.FishState>
{

    private bool doneIdel;

    private float waitRemaining ;
    public float waitTime = 1;

    public FishIdelState() : base(FishStateMachine.FishState.Idle) { 

    }

    public override void EnterState()
    {
        doneIdel = false;
        waitRemaining = waitTime;
    }

    public override void ExistState()
    {
    }

    public override void UpdateState()
    {
        waitRemaining -= Time.deltaTime;
        if (waitRemaining <= 0)
            doneIdel = true;
    }

    public override FishStateMachine.FishState GetNextState()
    {

        if (doneIdel)
            return FishStateMachine.FishState.Wander;
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
