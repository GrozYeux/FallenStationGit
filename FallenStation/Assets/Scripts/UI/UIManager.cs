using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : AbstractSingleton<UIManager>
{
    //le texte de feedback UI
    [SerializeField] private Text affichageCollectable = null; 
    // Start is called before the first frame update
    void Start()
    {
        affichageCollectable.gameObject.SetActive(false);
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
        affichageCollectable.gameObject.SetActive(true);
        affichageCollectable.text = "";
        int i;
        string res = text + " collecté";
        for(i=0; i<res.Length; i++)
        {
            affichageCollectable.text += res[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        affichageCollectable.gameObject.SetActive(false);
    }
}
