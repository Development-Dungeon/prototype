using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TunaController : MonoBehaviour
{

    public StateMachine stateMachine;
    Animator animator;
    public Collider container;
    public EnemyAttributes attributes;
    public EnemyDetection enemyDetection;

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    void Start()
    {
        stateMachine = new StateMachine();
        animator = gameObject.GetComponent<Animator>();
        enemyDetection = gameObject.GetComponent<EnemyDetection>();
        container = gameObject.transform.parent.gameObject.transform.Find("Container").GetComponent<Collider>();
        stateMachine.StateMachineNewStateEvent += UpdateTextForStateMachine;

        var idleState = new EnemyIdleState(gameObject, animator, attributes.pauseAfterMovementTime);
        var wanderState = new EnemyWanderState(gameObject, animator, container, attributes.moveSpeed, attributes.rotationSpeed, attributes.wanderDistanceRange);
        var chaseState = new EnemyChaseState(gameObject, animator, container, attributes.moveSpeed, attributes.rotationSpeed, enemyDetection);
        var attackState = new EnemyAttackState(gameObject, animator, enemyDetection, attributes.attackDamage, attributes.cooldown);

        At(idleState, chaseState, new FuncPredicate(() => enemyDetection.HasTarget()));
        At(idleState, wanderState, new FuncPredicate(() => idleState.cooldownTimer.IsFinished));
        At(wanderState, idleState, new FuncPredicate(() => wanderState.reachedDestination));
        At(wanderState, chaseState, new FuncPredicate(() => enemyDetection.targetWithinDetectionRange));
        At(chaseState, idleState, new FuncPredicate(() => !enemyDetection.HasTarget()));
        At(chaseState, attackState, new FuncPredicate(() => enemyDetection.targetWithinAttackRange));
        At(attackState, chaseState, new FuncPredicate(() => !enemyDetection.targetWithinAttackRange && enemyDetection.targetWithinDetectionRange));
        At(attackState, idleState, new FuncPredicate(() => !enemyDetection.targetWithinAttackRange && !enemyDetection.targetWithinDetectionRange));

        stateMachine.SetState(idleState);

    }

    private void UpdateTextForStateMachine(Type newState)
    {
        var textGO = transform.Find("canvasGO")?.GetComponent<TextMeshPro>();
        textGO.text = newState.Name;
    }

    private void OnDestroy() => stateMachine.StateMachineNewStateEvent -= UpdateTextForStateMachine;

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixUpdate();
    }
}
