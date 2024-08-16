using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEnemyFields : MonoBehaviour
{

    private EnemyAttributes attributes;
    private bool isDetected = false;

    void Start()
    {
        attributes = GetComponent<StateManager>().enemyAttributes;
        GetComponent<StateManager>().StatusChangeEvent += UpdateDetectionSphere;
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {

        DrawForward();
        DrawDetectionRange();
        DrawContainer();


    }

    private void DrawContainer()
    {
        // get the volum on the enemny
        VolumeAttributes va = GetComponent<VolumeAttributes>();

        if (va == null) return;

        GameObject container = va.container;

        if (container == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(container.transform.position, container.transform.lossyScale);
    }

    private void UpdateDetectionSphere(State currentState)
    {

        if (currentState.GetType().Name == "ChaseState")
        {
            isDetected = true;
        }
        else if (currentState.GetType().Name == "AttackState")
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
