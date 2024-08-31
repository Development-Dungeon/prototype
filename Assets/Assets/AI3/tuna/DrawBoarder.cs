using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoarder : MonoBehaviour
{
    // Start is called before the first frame update
    //private Collider container;
    private void Awake()
    {
        
        //container = gameObject;
    }
    void Start()
    {

    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;

        //Gizmos.DrawWireCube(container.transform.position, container.transform.lossyScale);
        Gizmos.DrawWireCube(gameObject.transform.position, gameObject.transform.lossyScale);
    }

    void Update()
    {

    }
}
