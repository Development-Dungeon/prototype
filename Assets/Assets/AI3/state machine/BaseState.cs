﻿using UnityEngine;

public abstract class BaseState : IState
{

    protected readonly GameObject player;
    protected readonly Animator animator;

    protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    protected static readonly int JumpHash = Animator.StringToHash("Jump");


    protected const float crossFadeDuration = 0.1f;

    protected BaseState(GameObject player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public virtual void FixeUpdate()
    {
        // noop
    }

    public virtual void OnEnter()
    {
        // noop
    }

    public virtual void OnExit()
    {
        // noop
    }

    public virtual void Update()
    {
        // noop
    }
}
