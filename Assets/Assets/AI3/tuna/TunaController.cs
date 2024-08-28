using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyDetection enemyDetection;

    public EnemyAttackState(GameObject enemy, Animator animator, EnemyDetection enemyDetection) : base(enemy, animator)
    {
        this.enemyDetection = enemyDetection;
    }

    public override void Update()
    {
        Debug.Log("attack");
        if (enemyDetection.targetWithinAttackRange && enemyDetection.targetGO != null)
        {
            enemyDetection.targetGO.SetActive(false);
        }

    }
}

public class TunaController : MonoBehaviour
{

    StateMachine stateMachine;
    Animator animator;
    public Collider container;
    public EnemyAttributes enemyAttributes;
    public EnemyDetection enemyDetection;


    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    void Start()
    {
        stateMachine = new StateMachine();
        animator = gameObject.GetComponent<Animator>();
        enemyDetection = gameObject.GetComponent<EnemyDetection>();

        var idleState = new EnemyIdleState(gameObject, animator, enemyAttributes.pauseAfterMovementTime);
        var wanderState = new EnemyWanderState(gameObject, animator, container, enemyAttributes.moveSpeed, enemyAttributes.rotationSpeed, enemyAttributes.wanderDistanceRange);
        var chaseState = new EnemyChaseState(gameObject, animator, container, enemyAttributes.moveSpeed, enemyAttributes.rotationSpeed, enemyDetection);
        var attackState = new EnemyAttackState(gameObject, animator, enemyDetection);

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

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixUpdate();
    }
}
