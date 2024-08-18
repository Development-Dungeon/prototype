using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishChaseState : BaseState<FishStateMachine.FishState>
{

    public FishChaseState() : base(FishStateMachine.FishState.Chase) {  }

    public override void EnterState(GameObject go)
    {
    }

    public override void UpdateState(GameObject go)
    {

        var speed = go.GetComponent<StateManager>().enemyAttributes.moveSpeed;
        var rotationSpeed = go.GetComponent<StateManager>().enemyAttributes.rotationSpeed;

        var step = speed * Time.deltaTime;
        var rotationStep = rotationSpeed * Time.deltaTime;


        var attackRange = go.GetComponent<StateManager>().enemyAttributes.attackRange;
        var detectionRange = go.GetComponent<StateManager>().enemyAttributes.enemyDetectionRange;

        GameObject player = DetectClosest(go.transform.position, detectionRange, "Player", LayerMask.NameToLayer("Player"));

        if (player == null)
            throw new System.Exception("could not find Player tag for Chase State calculations");

        Vector3 target = player.transform.position;

        if (Vector3.Distance(go.transform.position, target) <= attackRange)
        {
            return;
        }


        Vector3 nextStep = Vector3.MoveTowards(go.transform.position, target, step);
        Quaternion nextRotation = LookAt1(nextStep, go, 1f);

        // if next step is within the bounds of the container then take the step, otherwise do wait

        VolumeAttributes volumeAttributes = go.GetComponent<VolumeAttributes>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if (volumneCollider.bounds.Contains(nextStep))
        {
            go.transform.position = nextStep;
            go.transform.rotation = nextRotation;
        }
        else
        {
            Debug.Log("in chase, next step is not within the volumne");
        }

    }

    private Quaternion LookAt1(Vector3 wanderTarget, GameObject go, float rotationSpeed)
    {
        var target = wanderTarget - go.transform.position;

        var rotation = Quaternion.LookRotation(target);

        //go.transform.rotation = Quaternion.Slerp(go.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        return Quaternion.Slerp(go.transform.rotation, rotation, rotationSpeed);
    }

    public override void ExistState(GameObject g)
    {
    }

    public override FishStateMachine.FishState GetNextState(GameObject go)
    {
        var attackRange = go.GetComponent<StateManager>().enemyAttributes.attackRange;
        var detectionRange = go.GetComponent<StateManager>().enemyAttributes.enemyDetectionRange;

        GameObject player = DetectClosest(go.transform.position, detectionRange, "Player", LayerMask.NameToLayer("Player"));

        if (player == null)
            throw new System.Exception("could not find Player tag for Chase State calculations");

        Vector3 target = player.transform.position;


        if (Vector3.Distance(go.transform.position, target) <= attackRange)
        {
            return FishStateMachine.FishState.Attack;
        }

        return FishStateMachine.FishState.Chase;
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

}
