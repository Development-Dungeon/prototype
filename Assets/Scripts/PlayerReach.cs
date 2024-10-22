using System;
using UnityEngine;

public class PlayerReach : MonoBehaviour
{
    public float reachDistance = 5f;
    public event Action<PlayerReach> ReachUpdateEvent;

    public void SetReachDistance(float newReach)
    {
        if (newReach == reachDistance)
            return;

        reachDistance = newReach;

        if (ReachUpdateEvent != null)
            ReachUpdateEvent.Invoke(this);
    }
    public void Start()
    {
        if (ReachUpdateEvent != null)
            ReachUpdateEvent.Invoke(this);
    }


    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            //Debug.Log("Object hit: " + hit.collider.gameObject.name); 
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * reachDistance, Color.green);
        }
    }

    public bool IsRaycastHit(int additionalReach = 0)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        return Physics.Raycast(ray, out hit, additionalReach + reachDistance);
    }
}
