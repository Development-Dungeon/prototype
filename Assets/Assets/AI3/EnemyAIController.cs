using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleStateNew : BaseState
{
    public bool running;

    private float waitTime;
    private float remainingTime;

    public IdleStateNew(GameObject player, Animator animator, float waitTime) : base(player, animator)
    {
        this.waitTime = waitTime;
    }


    public override void OnEnter()
    {
        running = true;
        remainingTime = waitTime;
    }

    public override void Update()
    {
        if (remainingTime <= 0)
            running = false;
        else
            remainingTime -= Time.deltaTime;
    }

    public override void OnExit()
    {
        running = false;
    }

}

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
        var wr = go.GetComponent<StateManager>().enemyAttributes.wanderDistanceRange;

        Vector3 randomUnitsToMove = new Vector3(Random.Range(-wr, wr), Random.Range(-wr, wr), Random.Range(-wr, wr));

        // check if that spot is within the range
        Vector3 newPosition = go.transform.position + randomUnitsToMove;


        // if it is, set that location as the destination and start walking to it
        VolumeAttributes volumeAttributes = go.GetComponent<VolumeAttributes>();
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

        var m_speed = go.GetComponent<StateManager>().enemyAttributes.moveSpeed;
        var rotationSpeed = go.GetComponent<StateManager>().enemyAttributes.rotationSpeed;

        // walk towards the new location given delta time
        var step = m_speed * Time.deltaTime;

        go.transform.position = Vector3.MoveTowards(go.transform.position, wanderTarget, step);

        WorldUtils.LookAt1(go.transform, go.transform.position , wanderTarget, rotationSpeed);

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

public class ChaseStateNew : BaseState
{
    public ChaseStateNew(GameObject player, Animator animator) : base(player, animator)
    {
    }

    public override void Update()
    {
        // move toward the target
        var speed = player.GetComponent<StateManager>().enemyAttributes.moveSpeed;
        var rotationSpeed = player.GetComponent<StateManager>().enemyAttributes.rotationSpeed;

        var step = speed * Time.deltaTime;
        var rotationStep = rotationSpeed * Time.deltaTime;


        Vector3 target = player.transform.position;


        Vector3 nextStep = Vector3.MoveTowards(player.transform.position, target, step);
        Quaternion nextRotation = WorldUtils.LookAt1(player.transform, nextStep, target, 1f);

        // if next step is within the bounds of the container then take the step, otherwise do wait

        VolumeAttributes volumeAttributes = player.GetComponent<VolumeAttributes>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if (volumneCollider.bounds.Contains(nextStep))
        {
            player.transform.position = nextStep;
            player.transform.rotation = nextRotation;
        }
        else
        {
            Debug.Log("in chase, next step is not within the volumne");
        }

    }
}

public class AttackStateNew : BaseState
{
    public AttackStateNew(GameObject player, Animator animator) : base(player, animator)
    {
    }

    public override void Update()
    {
        Debug.Log("Inside Attack state");
    }
}


public class EnemyAIController : MonoBehaviour
{
    StateMachine stateMachine;
    bool targetWithinDetectionRange;
    Transform target;
    bool targetWithinAttackRange;

    void Awake()
    {
        stateMachine = new StateMachine();


        // lets make the real states and see how that goes
        var idleState = new IdleStateNew(gameObject, null, 1f);
        var wanderState = new WanderStateNew(gameObject, null);
        var chaseState = new ChaseStateNew(gameObject, null);
        var attackState = new AttackStateNew(gameObject, null);


        // how to get a time to check if its not running

        At(idleState, wanderState, new FuncPredicate(() => !idleState.running));
        At(wanderState, idleState, new FuncPredicate(() => wanderState.reachedDestination));
        At(wanderState, chaseState, new FuncPredicate(() => targetWithinDetectionRange));
        At(chaseState, idleState, new FuncPredicate(() => !targetWithinDetectionRange));
        At(chaseState, attackState, new FuncPredicate(() => targetWithinAttackRange));
        At(attackState, chaseState, new FuncPredicate(() => !targetWithinAttackRange && targetWithinDetectionRange));
        At(attackState, idleState, new FuncPredicate(() => !targetWithinAttackRange && !targetWithinDetectionRange));

        stateMachine.SetState(idleState);

    }


    public void FindTargetWithinRange()
    {
        // if there is already a target, regardless if there is another one which is closer, stay locked onto target 
        if (target != null)
        {
            targetWithinDetectionRange = IsWithinRange(transform.position, target.position, 10);
            targetWithinAttackRange = IsWithinRange(transform.position, target.position, 2);
            target = targetWithinDetectionRange ? target : null;

            if (targetWithinDetectionRange) return; // stay locked on to the current target
        }
        else
        {
            // find a new target if there is no target
            var detectedGO = WorldUtils.DetectClosest(transform.position, 10, "Player", 3);

            targetWithinDetectionRange = detectedGO != null;
            targetWithinAttackRange = targetWithinDetectionRange && IsWithinRange(transform.position, target.position, 2);

            target = detectedGO != null ? detectedGO.transform : null;
        }

    }

    public bool EnemyWithinAttackRange()
    {
        var detectedGO = WorldUtils.DetectClosest(transform.position, 10, "Player", 3);

        if (detectedGO == null)
            return false;

        return IsWithinRange(transform.position, detectedGO.transform.position, 2);

    }

    private bool IsWithinRange(Vector3 focus, Vector3 other, float range)
    {

        return Vector3.Distance(focus, other) <= range;
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    void Start()
    {
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        FindTargetWithinRange();
        stateMachine.FixUpdate();
    }
}
