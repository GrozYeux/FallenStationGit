using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    protected virtual void Update()
    {
        
    }

    public virtual void Action()
    {

    }

    public virtual bool CheckCompletness()
    {
        return false;
    }
}
