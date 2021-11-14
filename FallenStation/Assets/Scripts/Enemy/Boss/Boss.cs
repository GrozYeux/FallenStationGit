using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject lame1;
    [SerializeField] private GameObject lame2;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject robotShooter;
    [SerializeField] private GameObject robotCac;

    private string[] fullHp = new string[] { "attack", "attack", "call", "attack", "dash", "missile", "attack", "attack", "dash" };
    private string[] midHp = new string[] { "laser", "attack", "call2", "attack", "dash", "missile", "dash", "call2", "attack", "laser", "attack" };
    private string[] quarterHp = new string[] { "attack", "call3", "attack", "dash", "missile", "dash", "call3", "attack", "laser", "missile", "dash", "dash", "call3", "laser", "attack", "missile" };
    private string[] lowHp = new string[] { "attack", "call3", "dash", "laser", "missile" };

    public float dashSpeed;
    public float dashTime;
    public float speed;
    public float health;
    public float maxHealth;

    private NavMeshAgent navMeshAgent;
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
    private int i;
    private bool newHealth = false;
    private bool newState = true;
    private string state;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        navMeshAgent = GetComponent<NavMeshAgent>();
        lame1.SetActive(false);
        lame2.SetActive(false);
        laser.SetActive(false);
        health = maxHealth;
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0)
        {
            Debug.Log("Boss died");
            Destroy(gameObject);
        }
        navMeshAgent.stoppingDistance = 15f;
        distance = (player.transform.position - gameObject.transform.position).sqrMagnitude;
        if(distance > 20*20)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        if (rotate)
        {
            Rotate();
        }
        if (spin)
        {
            transform.Rotate(0, 3, 0, Space.Self);
        }
        if (newState)
        {
            state = NewState();
            newState = false;
            switch (state)
            {
                case "attack":
                    StartCoroutine(Attack());
                    break;
                case "dash":
                    StartCoroutine(Dash());
                    break;
                case "missile":
                    StartCoroutine(Missile());
                    break;
                case "call":
                    StartCoroutine(Call());
                    break;
                case "laser":
                    StartCoroutine(Laser());
                    break;
                case "call2":
                    StartCoroutine(Call2());
                    break;
                case "call3":
                    StartCoroutine(Call3());
                    break;
            }
        }
    }

    private string NewState()
    {
        string res = "";
        int rand = 0;
        if (newHealth)
        {
            i = 0;
            newHealth = false;
        }
        if(health > maxHealth / 2)
        {
            if(i >= fullHp.Length)
            {
                i = 0;
            }
            res = fullHp[i];
            i += 1;
            return res;
        }
        else if(health > maxHealth / 4)
        {
            if (i >= midHp.Length)
            {
                i = 0;
            }
            res = midHp[i];
            i += 1;
            return res;
        }
        else if(health > maxHealth / 10)
        {
            if (i >= quarterHp.Length)
            {
                i = 0;
            }
            res = quarterHp[i];
            i += 1;
            return res;
        }
        else
        {
            rand = Random.Range(0, 5);
            return lowHp[rand];
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);
        newState = true;
    }

    IEnumerator Dash()
    {
        bool stop = false;
        float distanceDash;
        speed = navMeshAgent.speed;
        navMeshAgent.speed = 0;
        startTime = Time.time;
        while (Time.time < dashTime + startTime && !stop)
        {
            controller.Move(gameObject.transform.forward * dashSpeed * Time.deltaTime);
            distanceDash = (player.transform.position - transform.position).sqrMagnitude;
            if (distanceDash < 3 * 3)
            {
                stop = true;
            }
            yield return null;
        }
        Debug.Log(stop);
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
        yield return new WaitForSeconds(2f);
        newState = true;
    }

    IEnumerator Laser()
    {
        speed = navMeshAgent.speed;
        navMeshAgent.speed = 0;
        laser.SetActive(true);
        yield return new WaitForSeconds(3f);
        laser.SetActive(false);
        navMeshAgent.speed = speed;
        newState = true;
    }

    IEnumerator Call()
    {
        var newRobot = Instantiate(robotShooter, transform.position, transform.rotation);
        newRobot.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    IEnumerator Call2()
    {
        var newRobot = Instantiate(robotShooter, transform.position, transform.rotation);
        newRobot.gameObject.SetActive(true);
        //var newRobot2 = Instantiate(robotCac, transform.position, transform.rotation);
        //newRobot2.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    IEnumerator Call3()
    {
        var newRobot = Instantiate(robotShooter, transform.position, transform.rotation);
        newRobot.gameObject.SetActive(true);
        var newRobot2 = Instantiate(robotShooter, transform.position, transform.rotation);
        newRobot2.gameObject.SetActive(true);
        var newRobot3 = Instantiate(robotShooter, transform.position, transform.rotation);
        newRobot3.gameObject.SetActive(true);
        //var newRobot4 = Instantiate(robotCac, transform.position, transform.rotation);
        //newRobot4.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    IEnumerator Missile()
    {
        var newMissile = Instantiate(missile, transform.position, transform.rotation);
        newMissile.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    public void TakeDamage(float damage)
    {
        float previousHealth = health;
        health -= damage;
        Debug.Log("Boss heatlh = " + health);
        if(previousHealth > maxHealth / 2 && health <= maxHealth / 2)
        {
            newHealth = true;
        }
        if (previousHealth > maxHealth / 4 && health <= maxHealth / 4)
        {
            newHealth = true;
        }
        if (previousHealth > maxHealth / 10 && health <= maxHealth / 10)
        {
            newHealth = true;
        }
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
