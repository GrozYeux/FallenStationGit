using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject lame1;
    [SerializeField] private GameObject lame2;

    public float dashSpeed;
    public float dashTime;
    public float speed;
    private NavMeshAgent navMeshAgent;
    private float time;
    private float startTime;
    private float distance;
    private float x;
    private float z;
    private float rotation;
    private float speedrotation = 1;
    private Quaternion from;
    private Quaternion to;
    private bool rotate = true;
    private bool spin;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        lame1.SetActive(false);
        lame2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.stoppingDistance = 15f;
        distance = (player.transform.position - gameObject.transform.position).sqrMagnitude;
        if(distance > 20*20)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        time += Time.deltaTime;
        if(time > 10)
        {
            StartCoroutine(Dash());
            time = 0;
        }
        if (rotate)
        {
            Rotate();
        }
        if (spin)
        {
            transform.Rotate(0, 3, 0, Space.Self);
        }
    }

    IEnumerator Dash()
    {
        speed = navMeshAgent.speed;
        navMeshAgent.speed = 0;
        startTime = Time.time;
        while (Time.time < dashTime + startTime)
        {
            controller.Move(gameObject.transform.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
        lame1.SetActive(true);
        lame2.SetActive(true);
        rotate = false;
        spin = true;
        yield return new WaitForSeconds(0.5f);
        lame1.SetActive(false);
        lame2.SetActive(false);
        rotate = true;
        spin = false;
        navMeshAgent.speed = speed;
        yield return null;
    }

    private void Rotate()
    {
        x = player.transform.position.x - transform.position.x;
        z = player.transform.position.z - transform.position.z;
        rotation = Mathf.Atan(x / z) * (180 / Mathf.PI);
        if (z < 0 && x >= 0)
        {
            rotation -= 180;
        }
        if (x < 0 && z < 0)
        {
            rotation += 180;
        }
        from = gameObject.transform.rotation;
        to = Quaternion.Euler(0, rotation, 0);
        gameObject.transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime * speedrotation);
    }
}
