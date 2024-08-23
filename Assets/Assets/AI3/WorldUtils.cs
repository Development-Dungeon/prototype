using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static Quaternion LookAt1(Transform transform,Vector3 startingPos, Vector3 target, float rotationSpeed)
    {
        var direction = startingPos - target;

        var rotation = Quaternion.LookRotation(direction);

        return Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

    }

}
