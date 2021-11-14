using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = GameManager.Instance.GetPlayer();
            CharacterCombat enemyCombat = GetComponent<CharacterCombat>();
            if(enemyCombat != null)
            {
                enemyCombat.Attack(player.GetComponent<CharacterStats>());
            }
        }
    }
}
