using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    //la liste des cartes collectees
    private string[] objectsOwned;
    //la liste des notes collectees,
    private string[] notesOwned ;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Start()
    {
        CodexData data = SaveSystem.LoadCodex();
        if (data != null)
        {
            foreach (string name in data.notes)
            {
                Collectables.Instance.AddNote(name);
            }
        }
        ObjectData data1 = SaveSystem.LoadObject();
        if(data1 != null)
        {
            foreach (string name in data1.objects)
            {
                Collectables.Instance.AddObject(name);
            }
        }
        objectsOwned = Collectables.Instance.ArrayObjects();
        notesOwned = Collectables.Instance.ArrayNotes();
        foreach (string name in notesOwned) {
            if (GameObject.Find(name))
            {
                Destroy(GameObject.Find(name));
            }
        }
        foreach (string name in objectsOwned)
        {
            if (GameObject.Find(name))
            {
                Destroy(GameObject.Find(name));
            }
        }
    }
}
