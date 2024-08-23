using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : BaseState
{

    public RoamState(GameObject player, Animator animator) : base(player, animator)
    {
    }


    public override void Update()
    {
        Debug.Log("Inside RoamState");
    }

}
