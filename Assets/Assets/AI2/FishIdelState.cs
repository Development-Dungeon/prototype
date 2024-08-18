using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishIdelState : BaseState<FishStateMachine.FishState>
{

    private bool doneIdel;

    private float waitRemaining ;
    public float waitTime = 0;

    public FishIdelState() : base(FishStateMachine.FishState.Idle) { 

    }

    public override void EnterState(GameObject go)
    {
        doneIdel = false;
        waitRemaining = waitTime;
    }

    public override void ExistState(GameObject go)
    {
    }

    public override void UpdateState(GameObject go)
    {
        waitRemaining -= Time.deltaTime;
        if (waitRemaining <= 0)
            doneIdel = true;
    }

    public override FishStateMachine.FishState GetNextState(GameObject go)
    {

        if (doneIdel)
            return FishStateMachine.FishState.Wander;
        return StateKey;
    }

    public override void OnTriggerEnter(GameObject go,Collider other)
    {
    }

    public override void OnTriggerExit(GameObject go,Collider other)
    {
    }

    public override void OnTriggerStay(GameObject go,Collider other)
    {
    }

}
