using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject textTitle;
    public GameObject textAuthor;
    public GameObject textDate;
    public GameObject textBody;

    private TextMesh mTitle;
    private TextMesh mAuthor;
    private TextMesh mDate;
    private TextMesh mBody;
    void Start()
    {
        mTitle = textTitle.GetComponent<TextMesh>();
        mAuthor = textAuthor.GetComponent<TextMesh>();
        mDate = textDate.GetComponent<TextMesh>();
        mBody = textBody.GetComponent<TextMesh>();

        Clear();

        mBody.text = "Appuie sur 1, 2 ou 3 du clavier alphanumérique pour charger les textes.";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) DisplayNote("demo/1");
        if (Input.GetKeyDown(KeyCode.Alpha2)) DisplayNote("demo/2");
        if (Input.GetKeyDown(KeyCode.Alpha3)) DisplayNote("demo/3");
    }

    void Clear()
    {
        mTitle.text = "";
        mAuthor.text = "";
        mDate.text = "";
        mBody.text = "";
    }

    void DisplayNote(string name)
    {
        Note n = NoteFileParser.Load("Assets/Resources/Notes/"+name+".xml");
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
