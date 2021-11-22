using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : CharacterStats
{
    [SerializeField]
    private Animator animator;
    public override void Die()
    {
        base.Die();

        animator.SetBool("die", true);
        Debug.Log(transform.name + " died.");
        if (this.gameObject.GetComponent<EnemyZombie>())
        {
            this.gameObject.GetComponent<NavMeshAgent>().baseOffset = -0.7f;
            this.gameObject.GetComponent<NavMeshAgent>().height = 0.5f;
            this.gameObject.GetComponent<CapsuleCollider>().center= new  Vector3(0,0,0);
            this.gameObject.GetComponent<CapsuleCollider>().direction = 2;
            Destroy(this.GetComponent<EnemyZombie>());
            Destroy(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void Hurt(float newAlpha)
    {
        animator.Play("Z_TakeDamage");
        
        //currentState = State.Chase;
        //hurt animation ?
    }
    protected override void Awake()
    {
        base.Awake();
    }
}
