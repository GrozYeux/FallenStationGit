using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterStats))]
public class EnemyRobotShooter : EnemyBase
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
    public float shootFrequency = 1.0f;
    private float shootDelta = 0.0f;
    private float TimeWalk = 0.0f;
    private float rotationSpeed = 0.5f;
    GameObject arme;
    private float distanceWithPlayer;
    CharacterStats myStats;
    private Vector3 previousPosition;
    public float curSpeed;

    protected override void Start()
    {
        base.Start();
        arme = GameObject.Find("Weapon");
        player = GameManager.Instance.GetPlayer();
        target = player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<CharacterStats>();
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

    private void FixedUpdate()
    {
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        UpdateAnimator(curSpeed);
    }

    void UpdateAnimator(float speed)
    {
        animator.SetFloat("speed", speed);
        animator.SetBool("Attack", currentState == State.Attack);

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
        if (TimeWalk <= 3) {
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
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
        Debug.Log("Enemy shoot");
        RaycastHit hit;

        bool hitsPlayer = false;

        bool hitsSomething = Physics.Raycast(transform.position, transform.forward, out hit, lookRadius + 30, layerMask);
        if (hitsSomething)
        {
            if (hit.collider.tag == "Player")
                hitsPlayer = true;
        }
        
        
        //Debug.DrawRay(arme.transform.position, transform.forward, Color.yellow);
        if (hitsPlayer)
        {
            GameObject objHit = hit.collider.gameObject;
            CharacterCombat enemyCombat = GetComponent<CharacterCombat>();

            if (enemyCombat != null)
            {
                Debug.Log("Touched player !");
                enemyCombat.Attack(player.GetComponent<CharacterStats>());

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
