using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCercle : Puzzle
{
    [SerializeField] PuzzleCercle dependance = null;

    private int rotation = 0;

    //permet de mélanger au hasard le cercle au début
    protected override void Start()
    {
        rotation = Random.Range(0,18);
        for (int i = 0; i < rotation; i++)
        {
            transform.Rotate(0, 0, 20);
        }
    }

    //permet de faire tourner un cercle et d'appeler ses dépendances si il y en a
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

    //permet de faire tourner une dépendance
    protected override void _Action()
    {
        transform.Rotate(0, 0, 20);
        CheckCompletness();
    }

    //permet de vérifier si le puzzle est terminé ou pas
    public override bool CheckCompletness()
    {
        return GetComponentInParent<PuzzleCercleManager>().CheckCompletness();
    }
}
