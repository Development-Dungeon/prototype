using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyChaseState : EnemyBaseState
{

    Collider container;
    float m_speed;
    float r_speed;
    EnemyDetection enemyDetection;
    

    public EnemyChaseState(GameObject enemy, Animator animator, Collider container, float m_speed, float r_speed, EnemyDetection enemyDetection) : base(enemy, animator)
    {
        this.container = container;
        this.m_speed = m_speed;
        this.r_speed = r_speed;
        this.enemyDetection = enemyDetection;
    }

    public override void Update()
    {
        if ( enemyDetection == null) return;

        var target = enemyDetection.targetGO;

        if (target == null) return; // target may be null depending on the frame of the state machine

        WorldUtils.Move(enemy, target.transform.position, m_speed, r_speed, container);

    }
}

public class TunaController : MonoBehaviour
{

    StateMachine stateMachine;
    Animator animator;
    Collider container;
    public EnemyAttributes enemyAttributes;
    public EnemyDetection enemyDetection;


    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    void Start()
    {
        stateMachine = new StateMachine();
        animator = gameObject.GetComponent<Animator>();

        var swimState = new EnemySwimState(gameObject, animator);
        var chaseState = new EnemyChaseState(gameObject, animator, container, enemyAttributes.moveSpeed, enemyAttributes.rotationSpeed, enemyDetection);

        At(swimState, chaseState, new FuncPredicate(() => enemyDetection.HasTarget()));
        At(chaseState, swimState, new FuncPredicate(() => !enemyDetection.HasTarget()));


        stateMachine.SetState(swimState);

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
