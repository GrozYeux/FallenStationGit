using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public float damage = 20;

    private void Start()
    {
    }
    public void Attack(CharacterStats targetStats)
    {
        targetStats.TakeDamage(damage);
    }
}
