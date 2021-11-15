using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private bool EnteredTrigger;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnteredTrigger = true;
            EnterAction();
        }
    }

    protected virtual void EnterAction()
    {
        print("Player entered");
    }

    public bool IsTriggered()
    {
        return EnteredTrigger;
    }

}
