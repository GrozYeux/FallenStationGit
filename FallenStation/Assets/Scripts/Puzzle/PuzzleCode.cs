using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleCode : Puzzle
{
    [SerializeField] private UI_InputWindow code;
    [SerializeField] private GameObject floatingTextPrefab;
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
                        GameObject Clone = Instantiate(floatingTextPrefab, code.okBtn.transform.position, Quaternion.identity, code.transform);
                        Clone.GetComponent<Animator>().Play("floatingText");
                        Destroy(Clone, 0.3f);
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
                            GameObject Clone = Instantiate(floatingTextPrefab, code.okBtn.transform.position, Quaternion.identity, code.transform);
                            Clone.GetComponent<Animator>().Play("floatingText");
                            Destroy(Clone, 0.3f);
                            Action();
                        }
                    }, UnityEngine.UI.InputField.ContentType.IntegerNumber, new string[] { "mark","wall", "beds" });
                break;
            case Step.done:
                code.End("Système activé:", "Elimation des cibles ", () =>
                 {
                     GameObject allZombies = GameObject.Find("KillZombies");
                    //Détruire tout les zombies 
                    foreach (EnemyStats zombie in allZombies.GetComponentsInChildren<EnemyStats>())
                     {
                         zombie.Die();
                         zombie.GetComponent<Animator>().Play("Z_RIP");
                     }
                 });
                
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
