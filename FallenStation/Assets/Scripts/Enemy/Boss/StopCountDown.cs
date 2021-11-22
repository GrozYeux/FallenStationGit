using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCountDown : MonoBehaviour
{
    GameObject countdown;
    // Start is called before the first frame update
    void Start()
    {
        countdown = GameObject.Find("Countdown");
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            countdown.GetComponent<Countdown>().StopCoundDown();
        }
    }
}
