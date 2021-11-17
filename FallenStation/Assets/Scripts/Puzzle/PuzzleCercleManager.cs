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
            if(Mathf.Round((allChildren[i].transform.rotation.z*180/Mathf.PI)) != 0)
            {
                res = false;
                Debug.Log(allChildren[i].transform.rotation.z);
            }
        }
        Debug.Log(res);
        return res;
    }
}
