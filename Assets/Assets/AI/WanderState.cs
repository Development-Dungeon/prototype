using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    Vector3 wanderDesintation = Vector3.zero;
    float m_speed = 2;
    float rotationSpeed = 1;
    public WaitState waitState;


    public State ChooseDestination(MonoBehaviour bot)
    {

        // pic a random spot 3 units around the player
        Vector3 randomUnitsToMove = new Vector3(Random.Range(-3,3), Random.Range(-3,3), Random.Range(-3,3));

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

    public State Move(MonoBehaviour bot)
    {

        // walk towards the new location given delta time
        var step = m_speed * Time.deltaTime;
        var rotationStep = rotationSpeed * Time.deltaTime;
        bot.transform.position = Vector3.MoveTowards(bot.transform.position, wanderDesintation, step);
        bot.transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards(bot.transform.position, wanderDesintation, rotationStep, 0.0f));

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

        if (wanderDesintation == Vector3.zero) 
            return ChooseDestination(bot);
        else
            return Move(bot);

    }

}
