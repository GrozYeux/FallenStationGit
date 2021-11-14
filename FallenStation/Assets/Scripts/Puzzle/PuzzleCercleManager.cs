using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCercleManager : Puzzle
{
    public override bool CheckCompletness()
    {
        bool res = true;

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        for(int i = 0; i < allChildren.Length; i++)
        {
            if(allChildren[i].transform.rotation.z != 0)
            {
                res = false;
            }
        }
        Debug.Log(res);
        return res;
    }
}
