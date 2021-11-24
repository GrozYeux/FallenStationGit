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
    [SerializeField] private LayerMask layerMask;

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
        if (health < 0)
        {
            GameObject countdown = GameObject.Find("Countdown");
            countdown.GetComponent<Countdown>().LaunchCoundDown();         
               
            Debug.Log("Boss died");
            Destroy(gameObject);
        }
        if (Mathf.Abs(player.transform.position.y - transform.position.y) > 3)
        {
            navMeshAgent.stoppingDistance = 4f;
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            navMeshAgent.stoppingDistance = 15f;
            distance = (player.transform.position - gameObject.transform.position).sqrMagnitude;
            if (distance > 20 * 20) //pour que le boss se rapproche du joueur
            {
                navMeshAgent.SetDestination(player.transform.position);
            }
        }
        if (rotate) //pour que le boss regarde le joueur
        {
            Rotate();
        }
        if (spin) //pour que le boss spin à la fin du dash
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
                    StartCoroutine(Attack(3));
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

    //lit la prochaine string du tableau correspondant aux hp du boss, i est l'indice actuel du tableau
    private string NewState()
    {
        string res = "";
        int rand = 0;
        if (newHealth) //reset i si il y a un nouveau pattern
        {
            i = 0;
            newHealth = false;
        }
        if(health > maxHealth / 2)
        {
            if(i >= fullHp.Length) //reset i si il est à la fin du pattern
            {
                i = 0;
            }
            res = fullHp[i];
            i += 1;
            return res;
        }
        else if(health > maxHealth / 4)
        {
            if (i >= midHp.Length) //reset i si il est à la fin du pattern
            {
                i = 0;
            }
            res = midHp[i];
            i += 1;
            return res;
        }
        else if(health > maxHealth / 10)
        {
            if (i >= quarterHp.Length) //reset i si il est à la fin du pattern
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
            return lowHp[rand]; //choisi un état aléatoire
        }
    }

    //l'attaque classique, avec un raycast qui tire i fois avec 0.5 secondes d'écart
    IEnumerator Attack(int i)
    {
        RaycastHit hit;
        while (i > 0)
        {
            Physics.Raycast(transform.position, transform.forward, out hit, 100f, layerMask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                GameObject objHit = hit.collider.gameObject;
                CharacterCombat enemyCombat = GetComponent<CharacterCombat>();

                if (enemyCombat != null)
                {
                    Debug.Log("Touched player !");
                    enemyCombat.Attack(player.GetComponent<CharacterStats>());
                }
            }
            yield return new WaitForSeconds(0.5f);
            i -= 1;
        }
        newState = true;
    }

    //la capacité du dash, le boss active les lames et spin à la fin
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

    //la capacité du laser
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

    //la capacité d'appel : 1 robot shooter
    IEnumerator Call()
    {
        SpawnRobotShooter(1);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    //la capacité d'appel2 : 1 robot shooter et 1 robot Cac
    IEnumerator Call2()
    {
        SpawnRobotShooter(1);
        SpawnRobotCac(1);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    //la capacité d'appel3 : 3 robots shooter et 1 robot Cac
    IEnumerator Call3()
    {
        SpawnRobotShooter(3);
        SpawnRobotCac(1);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    //la capacité missile
    IEnumerator Missile()
    {
        var newMissile = Instantiate(missile, transform.position, transform.rotation);
        newMissile.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        newState = true;
    }

    //permet au boss de prendre des dégâts et change newHealth si il passe un palier
    public void TakeDamage(float damage)
    {
        float previousHealth = health;
        health -= damage;
        Debug.Log("Boss heatlh = " + health);
        if(previousHealth > maxHealth / 2 && health <= maxHealth / 2)
        {
            newHealth = true;
        }
        else if (previousHealth > maxHealth / 4 && health <= maxHealth / 4)
        {
            newHealth = true;
        }
        else if (previousHealth > maxHealth / 10 && health <= maxHealth / 10)
        {
            newHealth = true;
        }
    }

    //permet de faire spawn nb robot shooter
    private void SpawnRobotShooter(int nb)
    {
        int rand;
        Vector3 position;
        for(int i = 0; i<nb; i++)
        {
            //setup la position
            rand = Random.Range(0, 2);
            if(rand == 0)
            {
                position = new Vector3(120, 1, 120);
            }
            else
            {
                position = new Vector3(81, 6, 120);
            }
            var newRobot = Instantiate(robotShooter, position, transform.rotation);
            newRobot.gameObject.SetActive(true);
            EnemyRobotShooter scriptEnemyRobotShooter =  newRobot.transform.GetComponent<EnemyRobotShooter>();
            scriptEnemyRobotShooter.lookRadius = 60;
        }
    }

    //permet de faire spawn nb robot Cac
    private void SpawnRobotCac(int nb)
    {
        int rand;
        Vector3 position;
        for (int i = 0; i < nb; i++)
        {
            //setup la position
            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                position = new Vector3(120, 1, 120);
            }
            else
            {
                position = new Vector3(81, 6, 120);
            }
            var newRobot = Instantiate(robotCac, position, transform.rotation);
            newRobot.gameObject.SetActive(true);
            EnemyRobotFight scriptEnemyRobotFight = newRobot.transform.GetComponent<EnemyRobotFight>();
            scriptEnemyRobotFight.lookRadius = 60;
        }
    }

    //permet au boss de se tourner vers le joueur
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
