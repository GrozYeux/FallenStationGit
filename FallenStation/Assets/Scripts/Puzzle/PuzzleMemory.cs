using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMemory : Puzzle
{
    [SerializeField] public string logo = "";

    public bool isOn = false;
    public bool canClick = true;
    private Transform screen;
    private Transform screenColor;

    protected override void Start()
    {
        screen = transform.GetChild(1);
        screenColor = transform.GetChild(2);
        screen.gameObject.SetActive(false);
        screenColor.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (!isOn)
        {
            screenColor.gameObject.SetActive(false);
            screen.gameObject.SetActive(false);
        }
    }

    //permet de retourner la carte
    public override void Action()
    {
        if (!isOn && canClick)
        {
            isOn = true;
            screenColor.gameObject.SetActive(true);
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
