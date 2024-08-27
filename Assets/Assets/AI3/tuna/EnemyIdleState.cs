
using UnityEngine;
using System;

public class EnemyIdleState : EnemyBaseState
{

    public readonly Utilities.CountdownTimer cooldownTimer;


    public EnemyIdleState(GameObject enemy, Animator animator, float cooldownTime) : base(enemy, animator)
    {
        cooldownTimer = new Utilities.CountdownTimer(cooldownTime);
    }

    public override void OnEnter()
    {
        cooldownTimer.Start();
        animator.CrossFade(SwimHash, crossFadeDuration);
    }

    public override void OnExit()
    {
        cooldownTimer.Stop();
    }

    public override void Update()
    {

    }

}
