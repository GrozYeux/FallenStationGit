using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRobotShooter : EnemyBase
{
    [SerializeField]
    private float lookRadius = 12f;
    [SerializeField]
    private LayerMask layerMask;
    Transform target;
    NavMeshAgent navMeshAgent;
    public bool seesPlayer = false;
    public float shootFrequency = 1.0f;
    private float shootDelta = 0.0f;
    GameObject arme;
    float distanceWithPlayer;

    protected override void Start()
    {
        base.Start();
        arme = GameObject.Find("Weapon");
        GameObject player = GameManager.Instance.getPlayer();
        target = player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();
        distanceWithPlayer = Vector3.Distance(target.position, transform.position);
        if (distanceWithPlayer <= lookRadius )
        {
            seesPlayer = true;
        } else {
            seesPlayer = false;
            currentState = State.Idle;
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
        if (distanceWithPlayer <= lookRadius)
        {
            seesPlayer = true;
            currentState = State.Chase;

        } 
    }

    protected override void ChaseState()
    {
        if (seesPlayer == false)
        {
            currentState = State.Idle;
            return;
        }

        if (distanceWithPlayer <= navMeshAgent.stoppingDistance)
        {
            //attack the target
            currentState = State.Attack;

        } else {
            navMeshAgent.SetDestination(target.position);
            faceTarget();
        }
    }

    protected override void AttackState()
    {
        //Check the player visibility
        if (seesPlayer == false) {
            currentState = State.Idle;
            return;
        } 

        //check distance 
        if (distanceWithPlayer <= lookRadius && distanceWithPlayer >= navMeshAgent.stoppingDistance)
        {
            currentState = State.Chase;
        }

        faceTarget();

        //Shoot the player, accounting the frequency
        if (shootDelta > shootFrequency) {
            Hit();
            shootDelta = 0.0f;
        } else {
            shootDelta += Time.deltaTime;
        }
    }

    protected override void Hit()
    {
        Debug.Log("shoot");
        RaycastHit hit;
        bool hitsPlayer = Physics.Raycast(arme.transform.position, transform.forward, out hit, lookRadius+30, layerMask);
        //Debug.DrawRay(arme.transform.position, transform.forward, Color.yellow);
        if (hitsPlayer)
        {
            GameObject objHit = hit.collider.gameObject;
            Debug.Log("Touched player !");
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius );
    }
}
