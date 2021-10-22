using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRobotShooter : EnemyBase
{
    [SerializeField]
    private float lookRadius = 10f;
    Transform target;
    NavMeshAgent navMeshAgent;

    protected override void Start()
    {
        GameObject player = GameManager.Instance.getPlayer();
        target = player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            navMeshAgent.SetDestination(target.position);

            if (distance <= navMeshAgent.stoppingDistance)
            {
                //attack the target
                faceTarget();
            }
        }
    }

    void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
