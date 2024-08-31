using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoarder : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix; // this will allow for roation

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one); // this is required if using locat to world matrix
    }

}
