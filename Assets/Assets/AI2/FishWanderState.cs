using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWanderState : BaseState<FishStateMachine.FishState>
{

    private bool wanderComplete;
    private Vector3 wanderTarget;

    public FishWanderState() : base(FishStateMachine.FishState.Wander)
    { }


    public override void UpdateState(GameObject go)
    {

        if (wanderTarget == Vector3.zero)
            ChooseDestination(go);
        else
            Move(go);

    }


    public void ChooseDestination(GameObject go)
    {

        // pic a random spot 3 units around the player
        var wr = go.GetComponent<StateManager>().enemyAttributes.wanderDistanceRange;

        Vector3 randomUnitsToMove = new Vector3(Random.Range(-wr, wr), Random.Range(-wr, wr), Random.Range(-wr, wr));

        // check if that spot is within the range
        Vector3 newPosition = go.transform.position + randomUnitsToMove;


        // if it is, set that location as the destination and start walking to it
        VolumeAttributes volumeAttributes = go.GetComponent<VolumeAttributes>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if (volumneCollider.bounds.Contains(newPosition))
        {
            wanderTarget = newPosition;
        }
        else
        {
            Debug.Log("did not find a point within the volume. ");
        }

    }
    private void LookAt1(GameObject go, float rotationSpeed)
    {
        var target = wanderTarget - go.transform.position;

        var rotation = Quaternion.LookRotation(target);

        go.transform.rotation = Quaternion.Slerp(go.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public void Move(GameObject go)
    {

        var m_speed = go.GetComponent<StateManager>().enemyAttributes.moveSpeed;
        var rotationSpeed = go.GetComponent<StateManager>().enemyAttributes.rotationSpeed;

        // walk towards the new location given delta time
        var step = m_speed * Time.deltaTime;

        go.transform.position = Vector3.MoveTowards(go.transform.position, wanderTarget, step);

        LookAt1(go, rotationSpeed);

        //bot.transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards(bot.transform.position, wanderDesintation, rotationStep, 1f));
        //var target = wanderDesintation - bot.transform.position;
        //bot.transform.rotation = new Quaternion(90,90,90,0);

        // if I am within a certain distance to the destination, consider it arrived and set the destination to null 
        if (Vector3.Distance(go.transform.position, wanderTarget) < .1f)
        {
            wanderTarget = Vector3.zero;
            wanderComplete = true;
        }

    }

    public override FishStateMachine.FishState GetNextState(GameObject go)
    {
        var detectionRadius = go.GetComponent<StateManager>().enemyAttributes.enemyDetectionRange;

        var closestEnemy = DetectClosest(go.transform.position, detectionRadius, "Player", 3);

        if (closestEnemy != null)
            return FishStateMachine.FishState.Chase;

        if (wanderComplete)
            return FishStateMachine.FishState.Idle;
        return StateKey;
    }

    public override void EnterState(GameObject go)
    {
        wanderTarget = Vector3.zero;
        wanderComplete = false;
    }

    public override void ExistState(GameObject g)
    {
    }

    public override void OnTriggerEnter(GameObject go, Collider other)
    {
    }

    public override void OnTriggerStay(GameObject go, Collider other)
    {
    }

    public override void OnTriggerExit(GameObject go, Collider other)
    {
    }
}
