using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmeTurret : MonoBehaviour
{
    [SerializeField] private EnemyTurret[] turretArray;

    public void OnClick()
    {
        foreach(EnemyTurret turret in turretArray)
        {
            turret.detectionRange = 0f;
        }
    }
}
