using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    protected override void Die()
    {
        //damagePanel.SetActive(false);
        base.Die();
        Debug.Log(transform.name + " died.");
        Destroy(gameObject);
    }

    protected override void Hurt(float newAlpha)
    {
        //hurt animation ?
    }
}
