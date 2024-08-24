using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(GameObject player, Animator animator) : base(player, animator)
    {
    }

    public override void Update()
    {

        // if the target is within range
        // set the active to false

        var aiController = player.GetComponent<EnemyAIController>();

        if (aiController == null) return;

        var target = aiController.targetGO;

        var attackRange = 2f;

        if(Vector3.Distance(player.transform.position, target.transform.position) <= attackRange)
        {
            aiController.targetGO.SetActive(false);
		}


    }
}
