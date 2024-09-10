using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SardineController : MonoBehaviour
{

    public StateMachine stateMachine;
    public Collider container;
    public EnemyAttributes attributes;
    public EnemyDetection enemyDetection;
    private Animator animator;

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    void Start()
    {
        stateMachine = new StateMachine();
        enemyDetection = gameObject.GetComponent<EnemyDetection>();
        container = gameObject.transform.parent.gameObject.transform.Find("Container").GetComponent<Collider>();
        animator = gameObject.GetComponent<Animator>();
        //stateMachine.StateMachineNewStateEvent += UpdateTextForStateMachine;

        var idleState = new EnemyIdleState(gameObject, animator, attributes.pauseAfterMovementTime);
        var wanderState = new EnemyWanderState(gameObject, animator, container, attributes.moveSpeed, attributes.rotationSpeed, attributes.wanderDistanceRange);
        var fleeState = new EnemyFleeState(gameObject, animator, container, attributes.moveSpeed, attributes.rotationSpeed, enemyDetection);

        At(idleState, wanderState, new FuncPredicate(() => idleState.cooldownTimer.IsFinished));
        At(wanderState, idleState, new FuncPredicate(() => wanderState.reachedDestination));
        At(fleeState, idleState, new FuncPredicate(() => !enemyDetection.targetWithinDetectionRange));
        Any(fleeState, new FuncPredicate(() => enemyDetection.targetWithinDetectionRange));

        stateMachine.SetState(idleState);
        
    }

    private void UpdateTextForStateMachine(Type newState)
    {
        TextMeshPro textGO = transform.Find("canvasGO")?.GetComponent<TextMeshPro>();
        textGO.text = newState.Name;
    }

    void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixUpdate();

    private void OnDestroy() => stateMachine.StateMachineNewStateEvent -= UpdateTextForStateMachine;
}
