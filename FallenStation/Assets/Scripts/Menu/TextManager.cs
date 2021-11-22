using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject textTitle;
    public GameObject textAuthor;
    public GameObject textDate;
    public GameObject textBody;

    private TextMeshProUGUI mTitle;
    private TextMeshProUGUI mAuthor;
    private TextMeshProUGUI mDate;
    private TextMeshProUGUI mBody;
    void Start()
    {
        mTitle = textTitle.GetComponent<TextMeshProUGUI>();
        mAuthor = textAuthor.GetComponent<TextMeshProUGUI>();
        mDate = textDate.GetComponent<TextMeshProUGUI>();
        mBody = textBody.GetComponent<TextMeshProUGUI>();

        Clear();

        //mBody.text = "Appuie sur 1, 2 ou 3 du clavier alphanumérique pour charger les textes.";
    }

    //private void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.Alpha1)) DisplayNote("demo/1");
        //if (Input.GetKeyDown(KeyCode.Alpha2)) DisplayNote("demo/2");
        //if (Input.GetKeyDown(KeyCode.Alpha3)) DisplayNote("demo/3");
       // if (Input.GetKeyDown(KeyCode.Alpha4)) DisplayNote("demo/4");
    //}

    void Clear()
    {
        mTitle.text = "";
        mAuthor.text = "";
        mDate.text = "";
        mBody.text = "";
    }

    public void DisplayNote(string text)
    {
        
        string name = text;
        Note n = NoteFileParser.Load(Resources.Load<TextAsset>("Notes/"+name));
        if (n == null)
        {
            Debug.LogError("Le chargement de '"+name+"' a échoué");
            Clear();
            return;
        }

        mTitle.text = n.title;
        mAuthor.text = n.author;
        mDate.text = n.date;
        mBody.text = n.body;
    }

}
