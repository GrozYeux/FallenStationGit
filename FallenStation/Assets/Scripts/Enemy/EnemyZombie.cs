using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterStats))]
public class EnemyZombie : EnemyBase
{
    [SerializeField]
    private float lookRadius = 17f;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private LayerMask layerMask;
    Transform target;
    GameObject player;
    NavMeshAgent navMeshAgent;
    public bool seesPlayer = false;
    public float hitFrequency = 1.0f;
    private float hitDelta = 0.0f;
    private float TimeWalk = 0.0f;
    private float rotationSpeed = 0.9f;
    private float distanceWithPlayer;
    CharacterStats myStats;


    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.GetPlayer();
        target = player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
        animator.Play("Z_Idle");
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
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }


    protected override void IdleState()
    {
        TimeWalk += Time.deltaTime;
        animator.Play("Z_Idle");
        if (TimeWalk <= 3) {
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
            animator.Play("Z_Walk");

        } else if(TimeWalk >= 4 && TimeWalk <= 5) {
            transform.Rotate(Vector3.up * Random.Range(90, 180) * rotationSpeed * Time.deltaTime);
        } else if(TimeWalk >= 6) {
            TimeWalk = 0.0f;
        }
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) , out hit, 2))
        {
            transform.Rotate(Vector3.up * Random.Range(90, 220) * rotationSpeed * Time.deltaTime);
        }

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
            animator.Play("Z_Run");
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

        //hit the player, accounting the frequency
        if (hitDelta > hitFrequency) {
            Hit();
            hitDelta = 0.0f;
        } else {
            hitDelta += Time.deltaTime;
        }
    }

    protected override void Hit()
    {
        Debug.Log("Enemy hit");
        RaycastHit hit;
        bool hitsPlayer = Physics.Raycast(transform.position, transform.forward, out hit, lookRadius+30, layerMask);
        if (hitsPlayer)
        {
            GameObject objHit = hit.collider.gameObject;
            CharacterCombat enemyCombat = GetComponent<CharacterCombat>();

            if (enemyCombat != null)
            {
                Debug.Log("Touched player !");
                enemyCombat.Attack(player.GetComponent<CharacterStats>());
                animator.Play("Z_Attack");

            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius );
    }

    /*public void setcurrentState(State newCurrentState)
    {
        currentState = newCurrentState;
    }*/
}
