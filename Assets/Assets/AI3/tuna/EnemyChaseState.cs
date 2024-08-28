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
        if (enemyDetection == null || enemyDetection.targetGO == null) return;

        WorldUtils.Move(enemy, enemyDetection.targetGO.transform.position, m_speed, r_speed, container);

    }
}
