
using UnityEngine;
using System;

public abstract class EnemyBaseState : IState
{

    protected TunaController enemyAIController;
    protected Animator animator;

    protected const float crossFadeDuration = .1f;

    protected static int SwimHash = Animator.StringToHash("Swim");

    protected EnemyBaseState(TunaController enemyAIController, Animator animator)
    {
        this.enemyAIController = enemyAIController;
        this.animator = animator;
    }

    public virtual void FixeUpdate()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void Update()
    {
    }
}
