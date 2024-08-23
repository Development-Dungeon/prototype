using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceState : BaseState
{
    public bool isDancing;
    private float duration = 3f;
    private float remaining;

    public DanceState(GameObject player, Animator animator) : base(player, animator)
    {

    }

    public override void OnEnter()
    {
        isDancing = true;
        remaining = duration;
    }

    public override void Update()
    {

        Debug.Log("inside dancestate");

        if (remaining >= 0)
            remaining -= Time.deltaTime;
        else
            isDancing = false;

    }


}
