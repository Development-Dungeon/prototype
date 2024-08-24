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


        // lets make the real states and see how that goes
        var idleState = new IdleStateNew(gameObject, null, 1f);
        var wanderState = new WanderStateNew(gameObject, null);
        var chaseState = new ChaseStateNew(gameObject, null);
        var attackState = new AttackStateNew(gameObject, null);


        // how to get a time to check if its not running

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

        // if there is already a target, regardless if there is another one which is closer, stay locked onto target 
        if (targetGO!= null && targetGO.activeSelf)
        {
            targetWithinDetectionRange = IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.enemyDetectionRange);
            targetWithinAttackRange = IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.attackRange);
            targetGO = targetWithinDetectionRange ? targetGO: null;

            if (targetWithinDetectionRange) return; // stay locked on to the current target
        }
        else
        {

            // find a new target if there is no target
            var detectedGO = WorldUtils.DetectClosest(transform.position, enemyAttributes.enemyDetectionRange, "Player", 3);

            targetWithinDetectionRange = detectedGO != null;
            targetWithinAttackRange = targetWithinDetectionRange && IsWithinRange(transform.position, detectedGO.transform.position, enemyAttributes.attackRange);

            targetGO = detectedGO;
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
