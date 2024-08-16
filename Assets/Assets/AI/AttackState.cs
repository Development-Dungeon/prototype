using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public IdleState idelState;
    public ChaseState chaseState;

    public override State RunCurrentState(MonoBehaviour bot)
    {
        // check if the enemny is still within reach to attack. if not move to the wait or 
        // find the player in the scene
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //if (players == null)
        //    return idelState;

        //// find the closest player
        //GameObject player = null;
        //float closestPlayer = Mathf.Infinity;

        //foreach (var p in players)
        //{
        //    var distanceFromBot = Vector3.Distance(bot.transform.position, p.transform.position);
        //    if (distanceFromBot < closestPlayer)
        //    {
        //        closestPlayer = distanceFromBot;
        //        player = p;
        //    }
        //}


        var attackRange = ((StateManager)bot).enemyAttributes.attackRange;

        GameObject player = DetectClosest(bot.transform.position, attackRange, "Player", LayerMask.NameToLayer("Player") );

        if (player == null)
            return idelState;

        var detectionRange = ((StateManager)bot).enemyAttributes.enemyDetectionRange;


        // if the player is further away then the reach distance then send it to the idel state
        if (Vector3.Distance(bot.transform.position, player.transform.position) >= attackRange)
        {
            if (Vector3.Distance(bot.transform.position, player.transform.position) <= detectionRange)
                return chaseState;
            else
                return idelState;
        }

        // destroy the player
        player.SetActive(false);

        return this;
    }
}
