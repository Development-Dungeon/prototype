using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoarder : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix; // this will allow for roation

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one); // this is required if using locat to world matrix
    }

    void Update()
    {

    }
}
