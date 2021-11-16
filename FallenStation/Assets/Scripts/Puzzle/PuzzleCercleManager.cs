using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCercleManager : Puzzle
{
    private bool carteCollectee = false;

    public override bool CheckCompletness()
    {
        bool res = true;

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        for(int i = 0; i < allChildren.Length; i++)
        {
            if(Mathf.Round((allChildren[i].transform.rotation.z*180/Mathf.PI)) != 0)
            {
                res = false;
            }
        }
        Debug.Log(res);
        if (!carteCollectee && res)
        {
            Collectables.Instance.AddObject("carte secrete");
            UITextManager.Instance.PrintText("Item carte secrete collecté");
            carteCollectee = true;
        }
        return res;
    }
}
