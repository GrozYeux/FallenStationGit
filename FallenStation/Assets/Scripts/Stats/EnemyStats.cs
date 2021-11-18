using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : CharacterStats
{
    [SerializeField]
    private Animator animator;
    protected override void Die()
    {
        base.Die();
        Debug.Log(transform.name + " died.");
        if (this.gameObject.GetComponent<EnemyZombie>())
        {
            animator.Play("Z_FallingBack");
            this.gameObject.GetComponent<NavMeshAgent>().baseOffset = 0;
            Destroy(this.gameObject.GetComponent<EnemyZombie>());
            Destroy(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void Hurt(float newAlpha)
    {
        if (this.gameObject.GetComponent<EnemyZombie>())
        {
            animator.Play("Z_FallingBack");
        }
        //currentState = State.Chase;
        //hurt animation ?
    }
    protected override void Awake()
    {
        base.Awake();
    }
}
