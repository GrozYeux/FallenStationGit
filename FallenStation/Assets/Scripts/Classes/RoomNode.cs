using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
   
    //Variables associées - à affecter dans l'éditeur (à la mano)
    [SerializeField]
    private RoomNode parent;
    [SerializeField]
    private RoomNode[] childrens;

    public Door entranceDoor;

    private bool isActive;
    public void SetActive(bool value)
    {
        isActive = value;
        gameObject.SetActive(value);
    }

    public RoomNode GetParent()
    {
        return parent;
    }

    public RoomNode[] GetChildrens()
    {
        return childrens;
    }
}
