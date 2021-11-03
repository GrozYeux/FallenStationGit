using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : AbstractSingleton<Collectables>
{
    //la liste des cartes collectees, referencees par leur nom
    private HashSet<string> objectsOwned = new HashSet<string> ();
    //la liste des notes collectees, referencees par leur id
    private HashSet<string> notesOwned = new HashSet<string>();
    //le nombre de chargeur encore disponibles
    private int amoClip = 0;

    //ajoute une nouvelle carte de collectee
    public void AddObject(string obj)
    {
        objectsOwned.Add(obj);
    }
    
    //renvoie true si la carte est collectee, false sinon
    public bool CheckObject(string obj)
    {
        return objectsOwned.Contains(obj);
    }

    //renvoie un array contenant tous les noms des objets collectees
    public string[] ArrayObjects()
    {
        string[] res = new string[objectsOwned.Count];
        objectsOwned.CopyTo(res);
        return res;
    }

    //ajoute une nouvelle note de collectee
    public void AddNote(string note)
    {
        notesOwned.Add(note);
    }

    //renvoie true si la note est collectee, false sinon
    public bool CheckNote(string note)
    {
        return notesOwned.Contains(note);
    }

    //renvoie un array contenant tous les id des notes collectees
    public string[] ArrayNotes()
    {
        string[] res = new string[notesOwned.Count];
        notesOwned.CopyTo(res);
        return res;
    }

}
