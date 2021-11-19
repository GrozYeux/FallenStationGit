using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleCode : Puzzle
{
    [SerializeField] private UI_InputWindow code;
    protected Step currentStep;
    protected enum Step
    {
        step_1,
        step_2,
        step_3,
        done
    }

    protected override void Start()
    {
        currentStep = Step.step_1;
    }
    public override void Action()
    {
        switch (currentStep)
        {
            case Step.step_1:
                code.Show("ENTER CODE", "Password :",
                (string inputText) =>
                {
                    if (CheckCode(inputText))
                    {
                        currentStep = Step.step_2;
                        Debug.Log("step_1 reussis");
                        Action();
                    }
                    else
                    {
                            //pop-up fenetre code erroné
                            Action();
                    }
                }, UnityEngine.UI.InputField.ContentType.Name, new string[] { "Note", "Kahu" });
                break;
            //You must power the system first 
            case Step.step_2:
                code.Indice("NO POWER", "Vous devez alimenter le system:","ROOM");
                // si levier activé dans le bon ordre 
                if (CheckCompletness())
                {
                    currentStep = Step.step_3;
                    Debug.Log("step_2 reussis");
                    Action();
                }
                break;
            //"Enter the code to activate the system"
            case Step.step_3:
                code.Show("ENTER CODE", "Pour activer le système :",
                    (string inputText) =>
                    {
                        if (CheckCode(inputText))
                        {
                            currentStep = Step.done;
                            Debug.Log("step_3 reussis");
                            Action();
                        }
                        else
                        {
                            //pop-up fenetre code erroné
                            Action();
                        }
                    }, UnityEngine.UI.InputField.ContentType.IntegerNumber, new string[] { "mark","wall", "beds" });
                break;
            case Step.done:
                //Détruire tout les zombies 
                break;
        }
            
            
     }
    

    public bool CheckCode(string code)
    {
       if(currentStep == Step.step_1)
        {
            return (code == "VOID");
        }
       if(currentStep == Step.step_3)
        {
            return (code == "4951");
        }
        return false;
    }

    public override bool CheckCompletness()
    {
        return GetComponentInParent<PuzzleLeverManager>().CheckCompletness();
    }

}
