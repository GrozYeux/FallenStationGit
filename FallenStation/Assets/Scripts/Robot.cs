using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovementScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
              navMeshAgent.SetDestination(player.transform.position);
            
        }
    }

}