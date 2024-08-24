using UnityEngine;

public class WanderStateNew : BaseState
{
    public bool reachedDestination;
    private Vector3 wanderTarget;

    public WanderStateNew(GameObject player, Animator animator) : base(player, animator)
    {
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
        // pick a destination and travel to it
        if (wanderTarget == Vector3.zero)
            ChooseDestination(player);
        else
            Move(player);
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


    public void Move(GameObject go)
    {

        var m_speed = go.GetComponent<EnemyAIController>().enemyAttributes.moveSpeed;
        var rotationSpeed = go.GetComponent<EnemyAIController>().enemyAttributes.rotationSpeed;

        // walk towards the new location given delta time
        var step = m_speed * Time.deltaTime;

        go.transform.position = Vector3.MoveTowards(go.transform.position, wanderTarget, step);

        WorldUtils.LookAt1(go.transform, go.transform.position, wanderTarget, rotationSpeed);

        //bot.transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards(bot.transform.position, wanderDesintation, rotationStep, 1f));
        //var target = wanderDesintation - bot.transform.position;
        //bot.transform.rotation = new Quaternion(90,90,90,0);

        // if I am within a certain distance to the destination, consider it arrived and set the destination to null 
        if (Vector3.Distance(go.transform.position, wanderTarget) < .1f)
        {
            wanderTarget = Vector3.zero;
            reachedDestination = true;
        }
    }

}
