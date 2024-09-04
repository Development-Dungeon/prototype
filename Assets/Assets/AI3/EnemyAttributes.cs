using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="enemy attributes", menuName ="enemy")]
public class EnemyAttributes : ScriptableObject 
{
    [Header("Detection")]
    public float enemyDetectionRange;
    public float attackRange;
    public float wanderDistanceRange;


    [Header("Speed")]
    public float moveSpeed;
    public float rotationSpeed;
    public float pauseAfterMovementTime;

    [Header("Attack")]
    public float attackDamage;
    public float cooldown;

}
