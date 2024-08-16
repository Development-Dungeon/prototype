using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    public WanderState wanderState;
    public ChaseState chaseState;

    public GameObject humanPlayer;

    private float waitRemaining = 0;

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
            var waitForSeconds = ((StateManager)bot).enemyAttributes.pauseAfterMovementTime;
            waitRemaining = waitForSeconds;
            return wanderState;

		}

    }

    private bool IsEnemyInRange(MonoBehaviour bot)
    {

        var detectionRange = ((StateManager)bot).enemyAttributes.enemyDetectionRange;

        var player = DetectClosest(bot.transform.position, detectionRange, "Player", LayerMask.NameToLayer("Player"));

        return player != null;

  //      if(Vector3.Distance(bot.transform.position, humanPlayer.gameObject.transform.position) <= detectionRange)
  //      {
  //          // check if there is a direct line of sight by shooting a raycast
  //          RaycastHit rayCastInfo;
  //          //Vector3 rayToTarget = bot.transform.position - humanPlayer.gameObject.transform.position;
  //          Vector3 rayToTarget = humanPlayer.gameObject.transform.position - bot.transform.position ;

  //          if (Physics.Raycast(bot.transform.position, rayToTarget, out rayCastInfo))
  //          {
  //              if (humanPlayer.transform.CompareTag(rayCastInfo.transform.tag))
  //                  return true;
		//    }
		//}

        //return false;
    }

}
