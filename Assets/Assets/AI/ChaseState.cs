using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public float attackRange = 2;
    public float speed = 5;

    public override State RunCurrentState(MonoBehaviour bot)
    {
        // find the target and follow 
        // look for the player tag? 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            throw new System.Exception("could not find Player tag for Chase State calculations");

        Vector3 target = player.transform.position;

        if(Vector3.Distance(bot.transform.position, target) <= attackRange)
        {
            return attackState;
		}

        var step = speed * Time.deltaTime;

        Vector3 nextStep = Vector3.MoveTowards(bot.transform.position, target, step);

        // if next step is within the bounds of the container then take the step, otherwise do wait

        VolumeAttributes volumeAttributes = bot.GetComponent<VolumeAttributes>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if(volumneCollider.bounds.Contains(nextStep))
        {
            bot.transform.position = nextStep;
		}
        else
        {
            Debug.Log("in chase, next step is not within the volumne");
		}


        return this;

    }
}
