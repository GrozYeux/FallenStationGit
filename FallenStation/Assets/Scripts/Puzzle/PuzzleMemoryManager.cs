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

    //permet de vérifier si le puzzle est terminé ou pas
    public override bool CheckCompletness()
    {
        bool res = true;
        lastCard = currentCard;
        currentCard = newCard;
        if(pair && lastCard != currentCard) //check si on retourne la carte de paire
        {
            res = false;
            pair = false;
            foreach (PuzzleMemory puzzle in cards)
            {
                if (puzzle.isOn)
                {
                    puzzle.transform.Rotate(0, 0, -20);
                }
                puzzle.isOn = false;
            }
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
}
