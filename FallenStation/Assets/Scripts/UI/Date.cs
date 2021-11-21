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
    Transform mois;

    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        TimeWarp = player.GetComponent<TimeWarp>();
        annee = transform.Find("Année");
        jour = transform.Find("Jour");
        mois = transform.Find("Mois");
    }

    
    void Update()
    {
        
        if(TimeWarp.inPast)
        {
            if (TimeWarp.pastLevel.name == "2850")
            {
                jour.GetComponent<Text>().text = "Jour 12";
                mois.GetComponent<Text>().text = "Mois 07";
                annee.GetComponent<Text>().text = "Année 2850";
            }
            else if (TimeWarp.pastLevel.name == "2862")
            {
                jour.GetComponent<Text>().text = "Jour 17";
                mois.GetComponent<Text>().text = "Mois 04";
                annee.GetComponent<Text>().text = "Année 2862";
            } 
            else if (TimeWarp.pastLevel.name == "2944")
            {
                jour.GetComponent<Text>().text = "Jour 09";
                mois.GetComponent<Text>().text = "Mois 12";
                annee.GetComponent<Text>().text = "Année 2944";
            }
        } else {
            jour.GetComponent<Text>().text = "Jour 23";
            mois.GetComponent<Text>().text = "Mois 08";
            annee.GetComponent<Text>().text = "Année 2976";
        }
    }


}
