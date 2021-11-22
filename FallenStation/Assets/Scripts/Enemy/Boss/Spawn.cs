using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        boss.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boss.SetActive(true);
        }
    }
}
