using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : AbstractSingleton<UIManager>
{
    //le texte de feedback UI
    [SerializeField] private Text affichageCollectable = null;

    //pour savoir s'il y a déjà un texte
    int line = 0;
    List<string> allText = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        affichageCollectable.gameObject.SetActive(false);
    }

    private void Update()
    {
        affichageCollectable.text = "";
        if (allText.Count != 0)
        {
            affichageCollectable.gameObject.SetActive(true);
            foreach (string s in allText)
            {
                affichageCollectable.text += s + "\n";
            }
        }
        else
        {
            affichageCollectable.gameObject.SetActive(false);
        }
    }

    //met à jour le texte à afficher
    public void SetText(string text)
    {
        affichageCollectable.text = text;
    }

    //permet d'afficher le texte
    public void PrintText(string text)
    {
        StartCoroutine(PrintTextCorou(text));
    }

    //coroutine pour afficher petit à petit le texte
    IEnumerator PrintTextCorou(string text)
    {
        int myline = line;
        line += 1;
        string affichage = "";
        allText.Add(affichage);
        int i;
        string res = text + " collecté";
        for(i=0; i<res.Length; i++)
        {
            myline = allText.FindIndex((string s) => s == affichage);
            affichage += res[i];
            allText[myline] = affichage;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        allText.Remove(res);
        line -= 1;
    }
}
