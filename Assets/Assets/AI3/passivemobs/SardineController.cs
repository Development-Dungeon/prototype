using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SardineController : MonoBehaviour
{

    StateMachine stateMachine;
    public Collider container;
    public EnemyAttributes attributes;
    public EnemyDetection enemyDetection;

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    void Start()
    {
        stateMachine = new StateMachine();
        enemyDetection = gameObject.GetComponent<EnemyDetection>();
        container = gameObject.transform.parent.gameObject.transform.Find("Container").GetComponent<Collider>();

        var idleState = new EnemyIdleState(gameObject, null, attributes.pauseAfterMovementTime);
        var wanderState = new EnemyWanderState(gameObject, null, container, attributes.moveSpeed, attributes.rotationSpeed, attributes.wanderDistanceRange);
        var fleeState = new EnemyFleeState(gameObject, null, container, attributes.moveSpeed, attributes.rotationSpeed, enemyDetection);

        //At(idleState, wanderState, new FuncPredicate(() => idleState.cooldownTimer.IsFinished));
        //At(wanderState, idleState, new FuncPredicate(() => wanderState.reachedDestination));
        At(fleeState, idleState, new FuncPredicate(() => !enemyDetection.targetWithinDetectionRange));
        Any(fleeState, new FuncPredicate(() => enemyDetection.targetWithinDetectionRange));

        stateMachine.SetState(idleState);
        
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixUpdate();
    }
}
