using UnityEngine;

public class EnemyFleeState : EnemyBaseState
{
    Collider container;
    float m_speed;
    float r_speed;
    EnemyDetection enemyDetection;

    public EnemyFleeState(GameObject enemy, Animator animator, Collider container, float m_speed, float r_speed, EnemyDetection enemyDetection) : base(enemy, animator)
    {
        this.container = container;
        this.m_speed = m_speed;
        this.r_speed = r_speed;
        this.enemyDetection = enemyDetection;
    }

    public override void Update()
    {
        if (enemyDetection == null || enemyDetection.targetGO == null) return;

        // calculate the direction the player is coming to me and then continue to flee in that direction
        var direction = enemy.transform.position - enemyDetection.targetGO.transform.position; 
        var fleeLocation = enemy.transform.position + direction.normalized * m_speed;
        WorldUtils.Move(enemy, fleeLocation, m_speed, r_speed, container);

    }
}
