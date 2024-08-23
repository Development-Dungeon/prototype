using UnityEngine;

public class LocomotionState : BaseState
{
    public LocomotionState(GameObject player, Animator animator) : base(player, animator)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Inside LocomotionState OnEnter()");
    }


    
}
