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
    protected State currentState ;

    protected virtual void IdleState()
    {
        Debug.Log("IdleState");
    }

    protected virtual void ChaseState()
    {
        Debug.Log("Chase");
    }
    protected virtual void AttackState()
    {
        Debug.Log("Attack");
    }
    protected virtual void Hit()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
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
    }
}
