using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;

    public override State RunCurrentState(MonoBehaviour bot)
    {
        // find the target and follow 
        // look for the player tag? 
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        var attackRange = ((StateManager)bot).enemyAttributes.attackRange;
        var detectionRange = ((StateManager)bot).enemyAttributes.enemyDetectionRange;

        GameObject player = DetectClosest(bot.transform.position, detectionRange, "Player", LayerMask.NameToLayer("Player") );

        if (player == null)
            throw new System.Exception("could not find Player tag for Chase State calculations");

        Vector3 target = player.transform.position;


        if(Vector3.Distance(bot.transform.position, target) <= attackRange)
        {
            return attackState;
		}

        var speed = ((StateManager)bot).enemyAttributes.moveSpeed;
        var rotationSpeed = ((StateManager)bot).enemyAttributes.rotationSpeed;

        var step = speed * Time.deltaTime;
        var rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 nextStep = Vector3.MoveTowards(bot.transform.position, target, step);
        Vector3 nextRotation = Vector3.RotateTowards(bot.transform.position, target, rotationStep, 0.0f);

        // if next step is within the bounds of the container then take the step, otherwise do wait

        VolumeAttributes volumeAttributes = bot.GetComponent<VolumeAttributes>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if(volumneCollider.bounds.Contains(nextStep))
        {
            bot.transform.position = nextStep;
            bot.transform.rotation = Quaternion.LookRotation(nextRotation);
		}
        else
        {
            Debug.Log("in chase, next step is not within the volumne");
		}


        return this;

    }
}
