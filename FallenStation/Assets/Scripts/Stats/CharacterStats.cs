using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float damage = 20;
    public float maxHealth = 100;
    public float currentHealth { get; private set; }
    void Start()
    {

    }
    void Awake()
    {
        currentHealth = maxHealth;
    }

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

    protected virtual void Die()
    {
        //die in some way
        // to be overwritten
    }
    protected virtual void Hurt(float Alpha)
    {
        //had been hurting
        // to be overwritten
    }

}