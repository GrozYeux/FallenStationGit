using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cible : MonoBehaviour
{
  
    public int maxHP =5;
    public int HP =0;

    private void Start()
    {
        HP = maxHP;
    }
    public void GetDamage(int damage)
    {
        HP -= damage;
        if(HP == 0)
        {
            Destroy(gameObject);
        }
    }
}
