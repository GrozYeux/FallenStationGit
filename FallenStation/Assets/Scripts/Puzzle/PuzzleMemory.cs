using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMemory : Puzzle
{
    [SerializeField] public string logo = "";

    public bool isOn = false;
    public bool canClick = true;
    private Transform screen;

    protected override void Start()
    {
        screen = transform.GetChild(1);
        screen.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (!isOn)
        {
            screen.gameObject.SetActive(false);
        }
    }

    //permet de retourner la carte
    public override void Action()
    {
        if (!isOn && canClick)
        {
            isOn = true;
            screen.gameObject.SetActive(true);
            CheckCompletness();
        } 
    }

    //permet de vérifier si le puzzle est terminé ou pas
    public override bool CheckCompletness()
    {
        GetComponentInParent<PuzzleMemoryManager>().newCard = logo;
        return GetComponentInParent<PuzzleMemoryManager>().CheckCompletness();
    }
}
