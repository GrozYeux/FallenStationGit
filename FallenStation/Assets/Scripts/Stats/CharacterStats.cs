using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    
    public float maxHealth = 100;
    public float currentHealth { get; protected set; }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage. He has " + currentHealth + " of currentHealth.");
        Hurt((maxHealth - currentHealth) / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //die in some way
        //overwritten

    }
    protected virtual void Hurt(float Alpha)
    {
        //had been hurting
        // overwritten
    }

    protected virtual void Awake()
    {
        //overwritten
        currentHealth = maxHealth;
    }

}
