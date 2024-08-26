using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(GameObject player, Animator animator) : base(player, animator)
    {
    }

    public override void Update()
    {
        var aiController = player.GetComponent<EnemyAIController>();

        if (aiController == null) return;

        var target = aiController.targetGO;

        if (target == null) return; // target may be null depending on the frame of the state machine

        var enemyAIController = player.GetComponent<EnemyAIController>();

        Collider volumeCollider  = enemyAIController.container.GetComponent<Collider>();

        var m_speed = player.GetComponent<EnemyAIController>().enemyAttributes.moveSpeed;
        var r_speed = player.GetComponent<EnemyAIController>().enemyAttributes.rotationSpeed;

        WorldUtils.Move(player, target.transform.position, m_speed, r_speed, volumeCollider);

    }
}
