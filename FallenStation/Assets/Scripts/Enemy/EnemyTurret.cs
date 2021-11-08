using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyBase
{
    public float detectionRange = 5.0f;
    public float shootFrequency = 1.0f;

    private float shootDelta = 0.0f;
    private Vector3 offset = new Vector3(0,0.5f,0);

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject alertOn, alertOff;

    private bool seesPlayer = false;


    //Display the surrounding radius around the turret in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void checkPlayerVisibility()
    {
        bool hitsPlayer = false;
        GameObject player = GameManager.Instance.GetPlayer();

        Vector3 ppos = player.transform.position + offset;
        Vector3 dir = (ppos - transform.position).normalized;

        //Shoot a raycast toward the player
        RaycastHit hit;
        bool hitsSomething = Physics.Raycast(transform.position, dir, out hit, detectionRange, layerMask);

        if (hitsSomething)
        {
            GameObject objHit = hit.collider.gameObject;
            if(objHit.tag == "Player")
            {
                hitsPlayer = true;
            }
        }

        //Draw a debug line in the editor only
        Color debugColor = hitsPlayer ? Color.red : (hitsSomething ? Color.blue : Color.white);
        Debug.DrawRay(transform.position, dir * detectionRange, debugColor);

        seesPlayer = hitsPlayer;
    }

    void rotateTowardsPlayer()
    {
        body.transform.LookAt(GameManager.Instance.GetPlayer().transform);
    }

    protected override void IdleState()
    {
        if (seesPlayer)
        {
            currentState = State.Attack;
            shootDelta = shootFrequency-0.5f;
        }
    }

    protected override void ChaseState()
    {
        Debug.Log("Turret in Chase");
    }

    protected override void AttackState()
    {
        //Check the player visibility
        if (seesPlayer == false)
        {
            currentState = State.Idle;
            return;
        }

        //Rotate toward player
        rotateTowardsPlayer();

        //Shoot the player, accounting the frequency
        if (shootDelta > shootFrequency)
        {
            shootDelta = 0.0f;
            Hit();
        } else
        {
            shootDelta += Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        //Will affect the seesPlayer variable
        bool wasVisible = seesPlayer;
        checkPlayerVisibility();

        //Change : change the alerts
        if (wasVisible != seesPlayer)
        {
            if (seesPlayer)
            {
                alertOff.SetActive(false);
                alertOn.SetActive(true);
            }
            else
            {
                alertOff.SetActive(true);
                alertOn.SetActive(false);
            }
        }
    }

    protected override void Hit()
    {
        GameObject bullet = (GameObject)Instantiate(projectile, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.SetActive(true);
        Collider bCollider = bullet.GetComponent<Collider>();
        if (bCollider)
        {
            Physics.IgnoreCollision(bCollider, body.GetComponent<Collider>());
        }
        Rigidbody rbody = bullet.GetComponent<Rigidbody>();
        if (rbody)
        { //Apply force on the bullet (ex: canon ball)
            bullet.GetComponent<Rigidbody>().AddForce(shootPoint.transform.forward * 1200, ForceMode.Acceleration);
        }
    }

    protected override void Start()
    {
        base.Start();
        shootDelta = 0.0f;
        Debug.Log("StartTurret");
    }

    protected override void Update()
    {
        base.Update();
    }
}
