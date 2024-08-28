
using UnityEngine;
using System;

public abstract class EnemyBaseState : IState
{

    protected GameObject enemy;
    protected Animator animator;

    protected const float crossFadeDuration = .1f;

    protected static int SwimHash = Animator.StringToHash("Swim");

    protected EnemyBaseState(GameObject enemy, Animator animator)
    {
        this.enemy= enemy;
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
