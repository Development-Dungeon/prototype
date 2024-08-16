using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    public WanderState wanderState;
    public ChaseState chaseState;

    public float waitForSeconds = 1;
    public float sightRange = 5;
    public GameObject humanPlayer;

    private float waitRemaining = 5;

    public override State RunCurrentState(MonoBehaviour bot)
    {
        // if player is in range
        if(IsEnemyInRange(bot))
        {
            return chaseState;
		}
        // wait for 3 seconds and then go back to wander
        if (waitRemaining >= 0)
        {
            waitRemaining -= Time.deltaTime;
            return this;
        }
        else
        {
            waitRemaining = waitForSeconds;
            return wanderState;

		}

    }

    private bool IsEnemyInRange(MonoBehaviour bot)
    {

        if(Vector3.Distance(bot.transform.position, humanPlayer.gameObject.transform.position) <= sightRange)
        {
            // check if there is a direct line of sight by shooting a raycast
            RaycastHit rayCastInfo;
            //Vector3 rayToTarget = bot.transform.position - humanPlayer.gameObject.transform.position;
            Vector3 rayToTarget = humanPlayer.gameObject.transform.position - bot.transform.position ;

            if (Physics.Raycast(bot.transform.position, rayToTarget, out rayCastInfo))
            {
                if (humanPlayer.transform.CompareTag(rayCastInfo.transform.tag))
                    return true;
		    }
		}

        return false;
    }

    void OnDrawGizmos()
    {
        var target = transform.position + transform.forward * sightRange;

        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target);
        }
    }
}
