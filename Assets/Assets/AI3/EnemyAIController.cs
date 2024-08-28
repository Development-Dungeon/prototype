using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAIController : MonoBehaviour
{
    StateMachine stateMachine;
    bool targetWithinDetectionRange;
    public GameObject targetGO;
    bool targetWithinAttackRange;
    public GameObject container; // the enemy cannot go outside this containers bounds
    public EnemyAttributes enemyAttributes;


    void Awake()
    {
        stateMachine = new StateMachine();


        var idleState = new IdleState(gameObject, null, 1f);
        var wanderState = new WanderState(gameObject, null);
        var chaseState = new ChaseState(gameObject, null);
        var attackState = new AttackState(gameObject, null);



        At(idleState, wanderState, new FuncPredicate(() => !idleState.running));
        At(wanderState, idleState, new FuncPredicate(() => wanderState.reachedDestination));
        At(wanderState, chaseState, new FuncPredicate(() => targetWithinDetectionRange));
        At(chaseState, idleState, new FuncPredicate(() => !targetWithinDetectionRange));
        At(chaseState, attackState, new FuncPredicate(() => targetWithinAttackRange));
        At(attackState, chaseState, new FuncPredicate(() => !targetWithinAttackRange && targetWithinDetectionRange));
        At(attackState, idleState, new FuncPredicate(() => !targetWithinAttackRange && !targetWithinDetectionRange));

        stateMachine.SetState(idleState);

    }


    public void FindTargetWithinRange()
    {
        // if i have a target but hes dead, then remove him from being a target
        if (targetGO == null || !targetGO.activeSelf)
        {
            targetGO = WorldUtils.DetectClosest(transform.position, enemyAttributes.enemyDetectionRange, "Player", 3);

            targetWithinDetectionRange = targetGO != null;
            targetWithinAttackRange = targetWithinDetectionRange && IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.attackRange);

        }
        else if (targetGO != null && targetGO.activeSelf)
        {

            // if i have a target, remain on the target while he is in range
            targetWithinDetectionRange = IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.enemyDetectionRange);
            targetWithinAttackRange = IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.attackRange);

            if (!targetWithinDetectionRange)
            {
                targetGO = null;
            }
        }

    }


    private bool IsWithinRange(Vector3 focus, Vector3 other, float range)
    {

        return Vector3.Distance(focus, other) <= range;
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    void Start()
    {
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        FindTargetWithinRange();
        stateMachine.FixUpdate();
    }
}
