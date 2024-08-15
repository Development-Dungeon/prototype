using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public float attachReachDistance = 2;
    public IdleState idelState;

    public override State RunCurrentState(MonoBehaviour bot)
    {
        // check if the enemny is still within reach to attack. if not move to the wait or 
        // find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return idelState;

        // if the player is further away then the reach distance then send it to the idel state
        if (Vector3.Distance(bot.transform.position, player.transform.position) >= attachReachDistance)
            return idelState;

        return this;
    }
}
