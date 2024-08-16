using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    Vector3 wanderDesintation = Vector3.zero;
    public WaitState waitState;
    public ChaseState chaseState;


    public State ChooseDestination(MonoBehaviour bot)
    {

        // pic a random spot 3 units around the player
        var wr= ((StateManager)bot).enemyAttributes.wanderDistanceRange;


        Vector3 randomUnitsToMove = new Vector3(Random.Range(-wr,wr), Random.Range(-wr,wr), Random.Range(-wr,wr));

        // check if that spot is within the range
        Vector3 newPosition = bot.transform.position + randomUnitsToMove;


        // if it is, set that location as the destination and start walking to it
        VolumeAttributes volumeAttributes = bot.GetComponent<VolumeAttributes>();
        Collider volumneCollider = volumeAttributes.container.GetComponent<Collider>();

        if(volumneCollider.bounds.Contains(newPosition))
        {
            wanderDesintation = newPosition;
		}
        else
        {
            Debug.Log("did not find a point within the volume. ");
		}

        return this;

    }

    private void LookAt1(MonoBehaviour bot)
    {  
		var target = wanderDesintation - bot.transform.position;

        var rotation = Quaternion.LookRotation(target);

        var rotationSpeed = ((StateManager)bot).enemyAttributes.rotationSpeed;

        bot.transform.rotation = Quaternion.Slerp(bot.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    private void LookAt2(MonoBehaviour bot)
    {  
        bot.transform.rotation = Quaternion.LookRotation(wanderDesintation - bot.transform.position);
    }



    public State Move(MonoBehaviour bot)
    {

        // walk towards the new location given delta time
        var m_speed = ((StateManager)bot).enemyAttributes.moveSpeed;
        var rotationSpeed = ((StateManager)bot).enemyAttributes.rotationSpeed;
        var step = m_speed * Time.deltaTime;

        bot.transform.position = Vector3.MoveTowards(bot.transform.position, wanderDesintation, step);

        LookAt1(bot);

        //bot.transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards(bot.transform.position, wanderDesintation, rotationStep, 1f));
        //var target = wanderDesintation - bot.transform.position;
        //bot.transform.rotation = new Quaternion(90,90,90,0);

        // if I am within a certain distance to the destination, consider it arrived and set the destination to null 
        if (Vector3.Distance(bot.transform.position, wanderDesintation) < .1f)
        {
            wanderDesintation = Vector3.zero;
            return waitState;
		} 

        return this;
    }

    public override State RunCurrentState(MonoBehaviour bot)
    {

        var detectionRadius = ((StateManager)bot).enemyAttributes.enemyDetectionRange;

        var closestEnemy = DetectClosest(bot.transform.position, detectionRadius, "Player", 3);

        if (closestEnemy != null)
            return chaseState;

        if (wanderDesintation == Vector3.zero) 
            return ChooseDestination(bot);
        else
            return Move(bot);

    }

}
