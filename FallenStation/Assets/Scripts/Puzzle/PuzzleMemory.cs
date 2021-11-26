using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMemory : Puzzle
{
    [SerializeField] public string logo = "";

    public bool isOn = false;

    //permet de retourner la carte
    public override void Action()
    {
        isOn = true;
        transform.Rotate(0, 0, 20);
        CheckCompletness();
    }

    //permet de v�rifier si le puzzle est termin� ou pas
    public override bool CheckCompletness()
    {
        GetComponentInParent<PuzzleMemoryManager>().newCard = logo;
        return GetComponentInParent<PuzzleMemoryManager>().CheckCompletness();
    }
}
