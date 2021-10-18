using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyBase
{

    protected override void IdleState()
    {
        Debug.Log("Turret in IdleState");
    }

    protected override void ChaseState()
    {
        Debug.Log("Turret in Chase");
    }
    protected override void AttackState()
    {
        Debug.Log("Turret in Attack");
        Hit();
    }
    protected override void Hit()
    {
 
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("StartTurret");
    }

    protected override void Update()
    {
        base.Update();
    }
}
