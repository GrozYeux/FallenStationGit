using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyBase
{
    public float detectionRange = 5.0f;
    public float shootFrequency = 2.0f;

    private float shootDelta = 0.0f;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GameObject body;

    //Display the surrounding radius around the turret in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    bool playerVisible()
    {
        GameObject player = GameManager.Instance.getPlayer();

        Vector3 ppos = player.transform.position;
        Vector3 dir = (ppos - transform.position).normalized;

        //Shoot a raycast toward the player
        RaycastHit hit;
        bool hitsPlayer = Physics.Raycast(transform.position, dir, out hit, detectionRange, layerMask);
        //Draw a debug line in the editor only
        Color debugColor = hitsPlayer ? Color.red : Color.blue;
        Debug.DrawRay(transform.position, dir * detectionRange, debugColor);

        return hitsPlayer;
    }

    void rotateTowardsPlayer()
    {
        body.transform.LookAt(GameManager.Instance.getPlayer().transform);
    }

    protected override void IdleState()
    {
        if (playerVisible())
        {
            currentState = State.Attack;
            shootDelta = 0.0f;
        }
    }

    protected override void ChaseState()
    {
        Debug.Log("Turret in Chase");
    }

    protected override void AttackState()
    {
        //Check the player visibility
        if (playerVisible() == false)
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
    protected override void Hit()
    {
        Debug.Log("I shoot !");
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
