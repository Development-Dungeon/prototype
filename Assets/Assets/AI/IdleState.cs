using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    public WanderState wanderState;

    public float waitForSeconds = 1;
    private float waitRemaining = 1;

    public override State RunCurrentState(MonoBehaviour character)
    {
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
    void OnDrawGizmosSelected()
    {
        var target = this.transform;

        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
