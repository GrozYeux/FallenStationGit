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
    public bool seesPlayer = false;
    public float shootFrequency = 1.0f;
    private float shootDelta = 0.0f;

    protected override void Start()
    {
        GameObject player = GameManager.Instance.getPlayer();
        target = player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius && distance >= navMeshAgent.stoppingDistance)
        {
            seesPlayer = true;
            ChaseState(); //a enlever plus tard

        } else if (distance <= navMeshAgent.stoppingDistance) {
            //attack the target
            AttackState();//a enlever plus tard

        } else {
            seesPlayer = false;
        }
    }

    void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }
    protected override void IdleState()
    {
        if (seesPlayer)
        {
            //currentState = State.Attack;
        }
    }

    protected override void ChaseState()
    {
        navMeshAgent.SetDestination(target.position);
        faceTarget();
    }

    protected override void AttackState()
    {
        Debug.Log("AttackState");

        //Check the player visibility
        if (seesPlayer == false)
        {
            currentState = State.Idle;
            return;
        }

        faceTarget();

        //Shoot the player, accounting the frequency
        if (shootDelta > shootFrequency)
        {
            shootDelta = 0.0f;
            Hit();
        }
        else
        {
            shootDelta += Time.deltaTime;
        }
    }

    protected override void Hit()
    {
        /*GameObject bullet = (GameObject)Instantiate(projectile, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.SetActive(true);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), body.GetComponent<Collider>());
        bullet.GetComponent<Rigidbody>().AddForce(shootPoint.transform.forward * 1200, ForceMode.Acceleration);*/
        Debug.Log("Hit");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
