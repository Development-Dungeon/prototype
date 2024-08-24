using UnityEngine;

public class ChaseStateNew : BaseState
{
    public ChaseStateNew(GameObject player, Animator animator) : base(player, animator)
    {
    }

    public override void Update()
    {
        // move toward the target
        var speed = player.GetComponent<EnemyAIController>().enemyAttributes.moveSpeed;
        var rotationSpeed = player.GetComponent<EnemyAIController>().enemyAttributes.rotationSpeed;

        var step = speed * Time.deltaTime;
        var rotationStep = rotationSpeed * Time.deltaTime;

        // the target is on the ai controller


        var aiController = player.GetComponent<EnemyAIController>();

        if (aiController == null)
            return;

        var target = aiController.targetGO;

        if (target == null) return; // target may be null depending on the frame of the state machine

        Vector3 nextStep = Vector3.MoveTowards(player.transform.position, target.transform.position, step);
        //Quaternion nextRotation = WorldUtils.LookAt1(player.transform, nextStep, target, 1f);

        // if next step is within the bounds of the container then take the step, otherwise do wait

        var volumeAttributes = player.GetComponent<EnemyAIController>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if (volumneCollider.bounds.Contains(nextStep))
        {
            player.transform.position = nextStep;
            //player.transform.rotation = nextRotation;
        }
        else
        {
            Debug.Log("in chase, next step is not within the volumne");
        }

    }
}
