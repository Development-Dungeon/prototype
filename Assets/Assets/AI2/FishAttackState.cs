using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttackState : BaseState<FishStateMachine.FishState>
{

    public FishAttackState() : base(FishStateMachine.FishState.Attack) {  }

    public override void EnterState(GameObject go)
    {
        var attackRange = go.GetComponent<StateManager>().enemyAttributes.attackRange;

        GameObject player = DetectClosest(go.transform.position, attackRange, "Player", LayerMask.NameToLayer("Player"));

        if (player == null)
            return;

        var detectionRange = go.GetComponent<StateManager>().enemyAttributes.enemyDetectionRange;

        // if the player is further away then the reach distance then send it to the idel state
        if (Vector3.Distance(go.transform.position, player.transform.position) <= attackRange)
        {
			player.SetActive(false);
        }

    }

    public override void ExistState(GameObject g)
    {
    }

    public override FishStateMachine.FishState GetNextState(GameObject go)
    {
        var attackRange = go.GetComponent<StateManager>().enemyAttributes.attackRange;

        GameObject player = DetectClosest(go.transform.position, attackRange, "Player", LayerMask.NameToLayer("Player"));

        if (player == null)
            return FishStateMachine.FishState.Idle;

        var detectionRange = go.GetComponent<StateManager>().enemyAttributes.enemyDetectionRange;

        // if the player is further away then the reach distance then send it to the idel state
        if (Vector3.Distance(go.transform.position, player.transform.position) >= attackRange)
        {
            if (Vector3.Distance(go.transform.position, player.transform.position) <= detectionRange)
                return FishStateMachine.FishState.Chase;
            else
                return FishStateMachine.FishState.Idle;
        }

        return StateKey; 

    }

    public override void OnTriggerEnter(GameObject go, Collider other)
    {
    }

    public override void OnTriggerExit(GameObject go, Collider other)
    {
    }

    public override void OnTriggerStay(GameObject go, Collider other)
    {
    }

    public override void UpdateState(GameObject g)
    {
    }
}
