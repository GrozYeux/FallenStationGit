using System.Collections;
using UnityEngine;


public class PuzzleCode : Puzzle
{
    [SerializeField] private UI_InputWindow code;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Action()
    {
        if (!CheckCompletness())
        {
           
            code.Show("Enter the password :");
            //You must power the system first 
            //"Enter the code to activate the system"
        }
    }

    public override bool CheckCompletness()
    {
        return true;
    }
}
