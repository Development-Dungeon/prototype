using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class IDrawEnemyFields : MonoBehaviour
{

    public EnemyAttributes attributes;
    private bool isDetected = false;
    public Collider container;



    void OnDrawGizmos()
    {

        DrawForward();
        DrawDetectionRange();
        DrawContainer();


    }

    private void DrawContainer()
    {
        if (container == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(container.transform.position, container.transform.lossyScale);
    }

    protected void UpdateDetectionSphere(Type currentState)
    {

        if (currentState.Name.Contains("Chase"))
        {
            isDetected = true;
        }
        else if (currentState.Name.Contains("Attack"))
        {
            isDetected = true;
        }
        else if (currentState.Name.Contains("Flee"))
        {
            isDetected = true;
        }
        else
        {
            isDetected = false;
        }
    }


    private void DrawDetectionRange()
    {
        // change the color when an enemy is detected
        // i should just subscribe to the event

        if (attributes == null)
            return;

        if (isDetected)
        {
            var sphereColor = Color.red;
            sphereColor.a = 0.5f;
            Gizmos.color = sphereColor;
        }
        else
        {
            var sphereColor = Color.blue;
            sphereColor.a = 0.5f;
            Gizmos.color = sphereColor;
        }


        Gizmos.DrawSphere(transform.position, attributes.enemyDetectionRange);
    }

    private void DrawForward()
    {
        var target = transform.position + transform.forward * 2;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target);
    }
}
