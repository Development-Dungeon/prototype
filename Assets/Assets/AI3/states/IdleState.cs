using UnityEngine;

public class IdleState : BaseState
{
    public bool running;

    private float waitTime;
    private float remainingTime;

    public IdleState(GameObject player, Animator animator, float waitTime) : base(player, animator)
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
