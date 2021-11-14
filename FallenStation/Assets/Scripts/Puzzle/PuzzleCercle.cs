using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCercle : Puzzle
{
    public override void Action()
    {
        transform.Rotate(0, 0, 90);
        CheckCompletness();
    }

    public override bool CheckCompletness()
    {
        return GetComponentInParent<PuzzleCercleManager>().CheckCompletness();
    }
}
