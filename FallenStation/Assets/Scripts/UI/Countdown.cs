using System.Collections;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float time = 30f;
    //bool CountDownOn = false;
    GameObject boss;
    GameObject player;
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
    }


    void Update()
    {

    }

    public void LaunchCoundDown()
    {
        StartCoroutine(timer());
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
