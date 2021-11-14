using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float centerx;
    public float centery;
    public float centerz;
    public float growSpeed;

    private float time;
    private int height;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.localScale = new Vector3(100 + time * growSpeed, 100 + time * growSpeed, 100);
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(100, 100, 100);
        time = 0;
        height = Random.Range(0,2);
        transform.localPosition = new Vector3(0, -0.3f*height, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = GameManager.Instance.GetPlayer();
            CharacterCombat enemyCombat = GetComponent<CharacterCombat>();
            if (enemyCombat != null)
            {
                enemyCombat.Attack(player.GetComponent<CharacterStats>());
            }
        }
    }
}
