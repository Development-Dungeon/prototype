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
    public static Quaternion LookAt1(Transform transform, Vector3 startingPos, Vector3 target, float rotationSpeed)
    {
        var direction = target - startingPos;

        var rotation = Quaternion.LookRotation(direction);

        return Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

    }

    public Quaternion LookAt2(Vector3 target, GameObject go, float rotationSpeed)
    {
        var direction = target - go.transform.position;

        var rotation = Quaternion.LookRotation(target);

        //go.transform.rotation = Quaternion.Slerp(go.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        return Quaternion.Slerp(go.transform.rotation, rotation, rotationSpeed);
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
