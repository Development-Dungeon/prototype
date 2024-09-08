using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldUtils 
{
    public static GameObject DetectClosest(Vector3 center, float radius, string tag, int layer)
    {

        Collider[] results = Physics.OverlapSphere(center, radius, layer);

        if (results == null)
            return null;

        float closestDistance = Mathf.Infinity;
        GameObject closestGO = null;

        foreach (var possibleMatch in results)
        {
            if (possibleMatch.tag == tag)
            {
                var distanceFromCenter = Vector3.Distance(center, possibleMatch.transform.position);
                if (distanceFromCenter > radius)
                    continue;

                if (distanceFromCenter < closestDistance)
                {
                    closestGO = possibleMatch.gameObject;
                    closestDistance = distanceFromCenter;
                }
                // TODO add line of sight
            }
        }

        return closestGO;

    }

    public static GameObject DetectClosest(List<GameObject> gameObjects, Vector3 fromPostion)
    {
        if (gameObjects == null || gameObjects.Count == 0) return null;

        float closestDistance = Mathf.Infinity;
        GameObject closestGO = null;

        foreach (var possibleMatch in gameObjects)
        {
			var distanceFromCenter = Vector3.Distance(fromPostion, possibleMatch.transform.position);

			if (distanceFromCenter < closestDistance)
			{
			    closestGO = possibleMatch.gameObject;
			    closestDistance = distanceFromCenter;
			}
        }

        return closestGO;
    }

    public static List<GameObject> DetectAllClosest(Vector3 center, float radius, int layer)
    {

        Collider[] results = Physics.OverlapSphere(center, radius, layer);

        if (results == null)
            return new List<GameObject>();

        return results
				    .ToList()
				    .Select((i) => i.gameObject)
				    .ToList();

    }


    public static Quaternion LookAt1(Transform transform, Vector3 startingPos, Vector3 target, float rotationSpeed)
    {
        var direction = target - startingPos;

        var rotation = Quaternion.LookRotation(direction);

        return Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

    }


    public static void Move(GameObject go, Vector3 target, float m_speed, float r_speed, Collider volumeCollider)
    {
        var walkStep = m_speed * Time.deltaTime;
        var rotationStep = r_speed * Time.deltaTime;

        var nextStep = Vector3.MoveTowards(go.transform.position, target, walkStep);
        var nextRotation = WorldUtils.LookAt1(go.transform, go.transform.position, target, rotationStep);

        // verify that the next step is within the bounds
        if (volumeCollider == null || PointWithin(nextStep, volumeCollider))
        {
            go.transform.SetPositionAndRotation(nextStep, nextRotation);
        }
        else
        {
            Debug.Log("in chase, next step is not within the volumne");
        }

    }

    public static bool PointWithin(Vector3 point, Collider volumeCollider)
    {  
        return volumeCollider.bounds.Contains(point);
    }
}
