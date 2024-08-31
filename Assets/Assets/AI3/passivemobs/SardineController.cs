using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFleeState : EnemyBaseState
{
    Collider container;
    float m_speed;
    float r_speed;
    EnemyDetection enemyDetection;

    public EnemyFleeState(GameObject enemy, Animator animator, Collider container, float m_speed, float r_speed, EnemyDetection enemyDetection) : base(enemy, animator)
    {
        this.container = container;
        this.m_speed = m_speed;
        this.r_speed = r_speed;
        this.enemyDetection = enemyDetection;
    }

    public override void Update()
    {
        if (enemyDetection == null || enemyDetection.targetGO == null) return;

        // calculate awway location
        // i want to find the angle that the player is coming towards me and then go in the opposite direction?
        var direction = enemy.transform.position - enemyDetection.targetGO.transform.position; // this is the direction to the player.. i need just the negative of this? Add this to my current transform
        var fleeLocation = enemy.transform.position + direction.normalized * m_speed;
        WorldUtils.Move(enemy, fleeLocation, m_speed, r_speed, container);

    }
}

public class SardineController : MonoBehaviour
{

    StateMachine stateMachine;
    public Collider container;
    public EnemyAttributes attributes;
    public EnemyDetection enemyDetection;

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    // create a mob that just floats around and runs from the player if i get close
    void Start()
    {
        stateMachine = new StateMachine();
        enemyDetection = gameObject.GetComponent<EnemyDetection>();
        container = gameObject.transform.parent.gameObject.transform.Find("Container").GetComponent<Collider>();

        var idleState = new EnemyIdleState(gameObject, null, attributes.pauseAfterMovementTime);
        var wanderState = new EnemyWanderState(gameObject, null, container, attributes.moveSpeed, attributes.rotationSpeed, attributes.wanderDistanceRange);
        var fleeState = new EnemyFleeState(gameObject, null, container, attributes.moveSpeed, attributes.rotationSpeed, enemyDetection);

        At(idleState, wanderState, new FuncPredicate(() => idleState.cooldownTimer.IsFinished));
        At(wanderState, idleState, new FuncPredicate(() => wanderState.reachedDestination));
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
