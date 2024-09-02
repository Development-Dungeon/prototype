using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyDetection enemyDetection;
    public float attackDamage;
    public Utilities.CountdownTimer attackCooldownTimer;

    public EnemyAttackState(GameObject enemy, Animator animator, EnemyDetection enemyDetection, float attackDamage, float coolDown) : base(enemy, animator)
    {
        this.enemyDetection = enemyDetection;
        this.attackDamage = attackDamage;
        attackCooldownTimer = new Utilities.CountdownTimer(coolDown);

    }

    public override void Update()
    {
        attackCooldownTimer.Tick(Time.deltaTime);

        if (attackCooldownTimer.IsRunning)
            return;

        if (enemyDetection.targetWithinAttackRange && enemyDetection.targetGO != null)
        {
            // get the targets health component and attack it
            var playerHealth = enemyDetection.targetGO.GetComponent<Health>();
            if (playerHealth == null) return;

            playerHealth.TakeDamage(attackDamage);
            attackCooldownTimer.Start();

        }

    }
}
