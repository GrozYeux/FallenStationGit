using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    protected override void Die()
    {
        base.Die();
        Debug.Log(transform.name + " died.");
        Destroy(gameObject);
    }

    protected override void Hurt(float newAlpha)
    {

        //currentState = State.Chase;
        //hurt animation ?
    }
    protected override void Awake()
    {
        base.Awake();
    }
}
