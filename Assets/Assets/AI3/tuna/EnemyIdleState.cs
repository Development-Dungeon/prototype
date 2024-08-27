
using UnityEngine;
using System;

public class EnemyIdleState : EnemyBaseState
{

    public bool running;
    private readonly float idleTime;
    private float remainingTime = 0;

    public EnemyIdleState(GameObject enemy, Animator animator, float idleTileToWait) : base(enemy, animator)
    {
        this.idleTime = idleTileToWait;
    }

    public override void OnEnter()
    {
        animator.CrossFade(SwimHash, crossFadeDuration);
        running = true;
        remainingTime = idleTime;
    }

    public override void OnExit()
    {
        running = false;
    }

    public override void Update()
    {

        Debug.Log("idle");
        if (!running) return;

        remainingTime -= Time.deltaTime;

        running = remainingTime >= 0;


    }

}
