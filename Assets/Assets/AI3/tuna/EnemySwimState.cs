
using UnityEngine;

public class EnemySwimState : EnemyBaseState
{
    public EnemySwimState(GameObject enemy, Animator animator) : base(enemy, animator)
    {
    }

    public override void OnEnter() {
        Debug.Log("swim");
        animator.CrossFade(SwimHash, crossFadeDuration);
    }
}
