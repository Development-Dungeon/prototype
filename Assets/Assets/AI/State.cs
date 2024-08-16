using UnityEngine;

public abstract class State : MonoBehaviour
{

    public abstract State RunCurrentState(MonoBehaviour character);

    public GameObject DetectClosest(Vector3 center, float radius, string tag, int layer)
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
}
