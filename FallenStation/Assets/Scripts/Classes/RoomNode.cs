using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
   
    //Variables associées - à affecter dans l'éditeur (à la mano)
    [SerializeField]
    private RoomNode parent;
    
    //La liste des enfants est initialisée au runtime
    private List<RoomNode> childrens;

    private Door entranceDoor;

    private bool isActive;

    public void Awake()
    {
        childrens = new List<RoomNode>();
    }
    public void Start()
    {
        if(parent != null)
        {
            parent.LinkChildren(this);
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool value)
    {
        isActive = value;
        gameObject.SetActive(value);
    }

    public void LinkChildren(RoomNode r)
    {
        childrens.Add(r);
    }

    public RoomNode GetParent()
    {
        return parent;
    }

    public List<RoomNode> GetChildrens()
    {
        return childrens;
    }

    public Door GetEntranceDoor()
    {
        return entranceDoor;
    }

    public void SetEntranceDoor(Door door)
    {
        entranceDoor = door;
    }
}
