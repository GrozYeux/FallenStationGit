using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected int hp;
    protected int nbDamage;
    protected enum State
    {
        Idle,
        Chase,
        Attack
    }
    protected State currentState;


    protected virtual void IdleState()
    {

    }

    protected virtual void ChaseState()
    {

    }
    protected virtual void AttackState()
    {

    }
    protected virtual void Hit()
    {
        //a voir si laisser
    }
    protected virtual void Die()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Debug.Log("StartBase");
        currentState = State.Idle;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState();
                break;
        }
        if (hp <= 0)
        {
            Die();
        }
    }
}
