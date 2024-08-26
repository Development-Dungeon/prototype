
using UnityEngine;

public class EnemySwimState : EnemyBaseState
{
    public EnemySwimState(TunaController enemyAIController, Animator animator) : base(enemyAIController, animator)
    {
    }

    public override void OnEnter() {
        Debug.Log("swim");
        animator.CrossFade(SwimHash, crossFadeDuration);
    }
}
