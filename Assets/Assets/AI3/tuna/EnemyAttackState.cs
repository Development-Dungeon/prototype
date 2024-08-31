using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyDetection enemyDetection;

    public EnemyAttackState(GameObject enemy, Animator animator, EnemyDetection enemyDetection) : base(enemy, animator)
    {
        this.enemyDetection = enemyDetection;
    }

    public override void Update()
    {
        if (enemyDetection.targetWithinAttackRange && enemyDetection.targetGO != null)
        {
            enemyDetection.targetGO.SetActive(false);
        }

    }
}
