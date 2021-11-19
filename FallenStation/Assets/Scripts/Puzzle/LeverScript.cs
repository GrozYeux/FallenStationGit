using System.Collections;
using UnityEngine;


    public class LeverScript : MonoBehaviour
    {

    [SerializeField] int levernumber;
    [SerializeField] PuzzleLeverManager Manager;

    
        public void leverNumber()
        {
        if (!Manager.CheckCompletness())
        {
            Debug.Log("pull:");
            Debug.Log(levernumber);
            gameObject.GetComponentInParent<Animator>().Play("Pull");
            Manager.pulls += 1;
            Manager.playerOrder += levernumber;
        }
        }
    }

        
