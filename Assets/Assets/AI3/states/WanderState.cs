using UnityEngine;

public class WanderState : BaseState
{
    public bool reachedDestination;
    private Vector3 wanderTarget;
    private EnemyAIController enemyAIController;

    public WanderState(GameObject player, Animator animator) : base(player, animator)
    {
        enemyAIController = player.GetComponent<EnemyAIController>();
    }

    public override void OnEnter()
    {
        wanderTarget = Vector3.zero;
        reachedDestination = false;
    }

    public override void OnExit()
    {
        wanderTarget = Vector3.zero;
        reachedDestination = false;
    }
    public override void Update()
    {
        Collider volumeCollider = enemyAIController.container.GetComponent<Collider>();
        var m_speed = enemyAIController.enemyAttributes.moveSpeed;
        var r_speed = enemyAIController.enemyAttributes.rotationSpeed;


        // pick a destination and travel to it
        if (wanderTarget == Vector3.zero)
            ChooseDestination(player);
        else
            WorldUtils.Move(player, wanderTarget,m_speed, r_speed, volumeCollider);

        // check if reached target
        if (Vector3.Distance(player.transform.position, wanderTarget) < .1f)
        {
            wanderTarget = Vector3.zero;
            reachedDestination = true;
        }
    }


    public void ChooseDestination(GameObject go)
    {

        // pic a random spot 3 units around the player
        var wr = go.GetComponent<EnemyAIController>().enemyAttributes.wanderDistanceRange;

        Vector3 randomUnitsToMove = new Vector3(Random.Range(-wr, wr), Random.Range(-wr, wr), Random.Range(-wr, wr));

        // check if that spot is within the range
        Vector3 newPosition = go.transform.position + randomUnitsToMove;


        // if it is, set that location as the destination and start walking to it
        var volumeAttributes = go.GetComponent<EnemyAIController>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if (volumneCollider.bounds.Contains(newPosition))
        {
            wanderTarget = newPosition;
        }
        else
        {
            Debug.Log("did not find a point within the volume. ");
        }

    }

}
