using System.Collections;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float time = 30f;
    GameObject boss;
    GameObject player;
    IEnumerator CoTimer;
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        CoTimer = timer();
    }


    void Update()
    {

    }

    public void LaunchCoundDown()
    {
        StartCoroutine(CoTimer);
    }
    public void StopCoundDown()
    {
        StopCoroutine(CoTimer);
        Debug.Log("in StopCoundDown");
    }

    public IEnumerator timer()
    {
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1f);
            GetComponent<TextMesh>().text = "Autodestruction dans " + time.ToString();
        }
        if (time == 0)
        {
            GetComponent<TextMesh>().text = "BOOM !";
            player.GetComponent<PlayerStats>().TakeDamage(500);
        }
    }
}
