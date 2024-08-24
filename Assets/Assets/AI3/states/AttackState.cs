using UnityEngine;

public class AttackState : BaseState
{
    private EnemyAIController aiController;

    public AttackState(GameObject player, Animator animator) : base(player, animator)
    {
        aiController = player.GetComponent<EnemyAIController>();
    }

    public override void Update()
    {

        if (aiController == null) return;

        var target = aiController.targetGO;

        if(Vector3.Distance(player.transform.position, target.transform.position) <= aiController.enemyAttributes.attackRange)
        {
            aiController.targetGO.SetActive(false);
		}


    }
}
