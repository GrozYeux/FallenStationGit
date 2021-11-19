using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Date : MonoBehaviour
{
    GameObject player;
    TimeWarp TimeWarp;
    bool inPast;
    Transform annee;
    Transform jour;

    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        TimeWarp = player.GetComponent<TimeWarp>();
        annee = transform.Find("Ann�e");
    }

    
    void Update()
    {
        
        if(TimeWarp.inPast)
        {
            if (TimeWarp.pastLevel.name == "2850")
            {
                annee.GetComponent<Text>().text = "Ann�e 2850";
            }
            else if (TimeWarp.pastLevel.name == "2862")
            {
                annee.GetComponent<Text>().text = "Ann�e 2862";
            } 
            else if (TimeWarp.pastLevel.name == "2944")
            {
                annee.GetComponent<Text>().text = "Ann�e 2944";
            }
        } else {
            annee.GetComponent<Text>().text = "Ann�e 2976";
        }
    }


}
