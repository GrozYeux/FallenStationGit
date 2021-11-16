using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCercle : Puzzle
{
    [SerializeField] PuzzleCercle dependance = null;

    private int rotation = 0;

    //permet de m�langer au hasard le cercle au d�but
    protected override void Start()
    {
        rotation = Random.Range(0,18);
        for (int i = 0; i < rotation; i++)
        {
            transform.Rotate(0, 0, 20);
        }
    }

    //permet de faire tourner un cercle et d'appeler ses d�pendances si il y en a
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

    //permet de faire tourner une d�pendance
    protected override void _Action()
    {
        transform.Rotate(0, 0, 20);
        CheckCompletness();
    }

    //permet de v�rifier si le puzzle est termin� ou pas
    public override bool CheckCompletness()
    {
        return GetComponentInParent<PuzzleCercleManager>().CheckCompletness();
    }
}
