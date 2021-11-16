using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCercle : Puzzle
{
    [SerializeField] PuzzleCercle dependance = null;

    private int rotation = 0;

    protected override void Start()
    {
        rotation = Random.Range(0,18);
        for (int i = 0; i < rotation; i++)
        {
            transform.Rotate(0, 0, 20);
        }
    }

    public override void Action()
    {
        transform.Rotate(0, 0, 20);
        if(dependance != null)
        {
            dependance._Action();
        }
        else
        {
            CheckCompletness();
        }
    }

    public override void _Action()
    {
        transform.Rotate(0, 0, 20);
        CheckCompletness();
    }

    public override bool CheckCompletness()
    {
        return GetComponentInParent<PuzzleCercleManager>().CheckCompletness();
    }
}
