using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState<EState> where EState : Enum
{

    public BaseState(EState key) {
        StateKey = key;
    }

    public abstract void EnterState(GameObject go);
    public EState StateKey { get; private set; }
    public abstract void ExistState(GameObject go);
    public abstract void UpdateState(GameObject go);
    public abstract EState GetNextState(GameObject go);
    public abstract void OnTriggerEnter(GameObject go, Collider other);
    public abstract void OnTriggerStay(GameObject go, Collider other);
    public abstract void OnTriggerExit(GameObject go, Collider other);


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
