using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{

    public Vector3 wanderTarget;
    private Collider container;
    private readonly float m_speed;
    private readonly float r_speed;
    public bool reachedDestination;
    private readonly float wanderDistance;

    public EnemyWanderState(GameObject enemy, Animator animator, Collider container, float m_speed, float r_speed, float wanderDistance) : base(enemy, animator)
    {
        this.container = container;
        this.m_speed = m_speed;
        this.r_speed = r_speed;
        this.wanderDistance = wanderDistance;
    }


    public override void OnEnter()
    {
        wanderTarget = Vector3.zero;
        reachedDestination = false;
        animator.CrossFade(SwimHash, crossFadeDuration);
    }
    public override void OnExit()
    {
        wanderTarget = Vector3.zero;
        reachedDestination = false;
    }


    public override void FixeUpdate()
    {
        Debug.Log("wander");
        // pick a destination and travel to it
        if (wanderTarget == Vector3.zero)
            ChooseDestination(enemy);
        else
            WorldUtils.Move(enemy, wanderTarget, m_speed, r_speed, container);

        // check if reached target
        if (Vector3.Distance(enemy.transform.position, wanderTarget) < .1f)
        {
            wanderTarget = Vector3.zero;
            reachedDestination = true;
        }
    }

    public void ChooseDestination(GameObject go)
    {

        // pic a random spot 3 units around the player
        var wr = wanderDistance;

        Vector3 randomUnitsToMove = new Vector3(Random.Range(-wr, wr), Random.Range(-wr, wr), Random.Range(-wr, wr));

        // check if that spot is within the range
        Vector3 newPosition = go.transform.position + randomUnitsToMove;

        // if it is, set that location as the destination and start walking to it
        if (container.bounds.Contains(newPosition))
        {
            wanderTarget = newPosition;
        }
        else
        {
            Debug.Log("did not find a point within the volume. ");
        }
    }
}
