using System.Collections;
using UnityEngine;


public class Mushroom : MonoBehaviour
{
    [SerializeField] private GameObject spore;
    float time;

    private void Start()
    {
        spore.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 3f)
        {
            StartCoroutine(Spore());
            time = 0;
        }
    }
    IEnumerator Spore()
    {
        spore.SetActive(true);
        yield return new WaitForSeconds(3f);
        spore.SetActive(false);
    }
}
