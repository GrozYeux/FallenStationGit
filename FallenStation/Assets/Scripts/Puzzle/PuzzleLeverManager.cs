using System.Collections;
using UnityEngine;

public class PuzzleLeverManager : Puzzle
{
    public string playerOrder;
    public string leverOrder;

    public int pullLimit;
    public int pulls;

    



    private bool completed = false;


    // Update is called once per frame
    protected override void Update()
    {
        if (pulls >= pullLimit)
        {
            LeverCheck();
        }
    }

    public void LeverReset()
    {
        pulls = 0;
        playerOrder = "";
        foreach (GameObject lever in  GameObject.FindGameObjectsWithTag("lever"))
        {
            lever.gameObject.GetComponentInParent<Animator>().Play("Reset");
        }
    }

    public void LeverCheck()
    {
        if (playerOrder == leverOrder)
        {
            pulls = 0;
            completed = true;




        }
        else
        {
            LeverReset();
        }
    }
    


    public override bool CheckCompletness()
    {
        return completed;
    }
}
