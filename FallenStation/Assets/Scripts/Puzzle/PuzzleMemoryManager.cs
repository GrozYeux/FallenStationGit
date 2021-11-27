using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMemoryManager : Puzzle
{
    [SerializeField] private PuzzleMemory[] cards;

    //la nouvelle carte
    public string newCard = "";

    //la carte précédente
    private string lastCard = "";
    //la carte actuelle
    private string currentCard = "";
    //le booléen pour savoir si il faut une paire
    private bool pair = false;

    protected override void Start()
    {
        (Vector3,Quaternion)[] array = new (Vector3,Quaternion)[cards.Length];
        int i = 0;
        foreach (PuzzleMemory puzzle in cards)
        {
            array[i] = (puzzle.transform.position,puzzle.transform.rotation);
            i++;
        }
        for (int t = 0; t < array.Length; t++)
        {
            (Vector3, Quaternion) tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        i = 0;
        foreach (PuzzleMemory puzzle in cards)
        {
            puzzle.transform.SetPositionAndRotation(array[i].Item1,array[i].Item2);
            i++;
        }
    }

    //permet de vérifier si le puzzle est terminé ou pas
    public override bool CheckCompletness()
    {
        bool res = true;
        lastCard = currentCard;
        currentCard = newCard;
        if(pair && lastCard != currentCard) //check si on retourne la carte de paire
        {
            StartCoroutine(Wait());
            res = false;
        }
        else
        {
            foreach(PuzzleMemory puzzle in cards) //check si toute les cartes sont retournées
            {
                if (!puzzle.isOn)
                {
                    res = false;
                }
            }
            pair = !pair;
        }
        Debug.Log(res);
        return res;
    }

    protected override IEnumerator Wait()
    {
        foreach (PuzzleMemory puzzle in cards)
        {
            puzzle.canClick = false;
        }
        yield return new WaitForSeconds(0.7f);
        pair = false;
        foreach (PuzzleMemory puzzle in cards)
        {
            puzzle.isOn = false;
            puzzle.canClick = true;
        }
    }
}
