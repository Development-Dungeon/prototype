using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{

    public GameObject targetGO;
    public float enemyDetectionRange;
    [HideInInspector] EnemyAttributes enemyAttributes;
    public bool targetWithinDetectionRange;
    public bool targetWithinAttackRange;


    // Start is called before the first frame update
    void Start()
    {
        enemyAttributes = gameObject.GetComponent<EnemyAttributes>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        FindTargetWithinRange();
    }

    public bool HasTarget()
    {
        return targetGO != null;
    }

    private bool IsWithinRange(Vector3 focus, Vector3 other, float range)
    {

        return Vector3.Distance(focus, other) <= range;
    }


    public void FindTargetWithinRange()
    {
        // if i have a target but hes dead, then remove him from being a target
        if (targetGO == null || !targetGO.activeSelf)
        {
            targetGO = WorldUtils.DetectClosest(transform.position, enemyAttributes.enemyDetectionRange, "Player", 3);

            targetWithinDetectionRange = targetGO != null;
            targetWithinAttackRange = targetWithinDetectionRange && IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.attackRange);

        }
        else if (targetGO != null && targetGO.activeSelf)
        {

            // if i have a target, remain on the target while he is in range
            targetWithinDetectionRange = IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.enemyDetectionRange);
            targetWithinAttackRange = IsWithinRange(transform.position, targetGO.transform.position, enemyAttributes.attackRange);

            if (!targetWithinDetectionRange)
            {
                targetGO = null;
            }
        }

    }
}
