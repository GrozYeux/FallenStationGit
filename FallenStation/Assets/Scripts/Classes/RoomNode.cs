using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
   
    //Variables associées - à affecter dans l'éditeur (à la mano)
    [SerializeField]
    private RoomNode parent;
    [SerializeField]
    private RoomNode equivalentRoom;
    public bool noUnloadWithParent = false;

    //La liste des enfants est initialisée au runtime
    private List<RoomNode> childrens;

    public Door entranceDoor;

    private bool isActive;

    /* Affichage dans l'éditeur */
    void OnDrawGizmosSelected()
    {
        if (equivalentRoom != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(gameObject.transform.position, equivalentRoom.gameObject.transform.position);
            Gizmos.DrawSphere(equivalentRoom.gameObject.transform.position, 0.5f);
        }
        if (parent != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(gameObject.transform.position, parent.gameObject.transform.position);
        }
        if (entranceDoor != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(entranceDoor.gameObject.transform.position, Vector3.one);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(gameObject.transform.position, 0.5f);
    }

    public void Awake()
    {
        childrens = new List<RoomNode>();
    }
    public void Start()
    {
        if (parent != null)
        {
            parent.LinkChildren(this);
        }
        if(RoomsTreeManager.Instance.GetCurrentRoom() != this)
        {
            this.SetActive(false);
        }
        if (this.parent == RoomsTreeManager.Instance.GetCurrentRoom() && noUnloadWithParent)
            this.SetActive(this.parent.IsActive());
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool value)
    {
        if (value == false && this.parent == RoomsTreeManager.Instance.GetCurrentRoom() && noUnloadWithParent)
            return;

        isActive = value;
        gameObject.SetActive(value);
    }

    public void LinkChildren(RoomNode r)
    {
        if(r != null)
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

    public void SetEquivalentRoom(RoomNode r)
    {
        equivalentRoom = r;
    }

    public RoomNode GetEquivalentRoom()
    {
        print("Equivalent room : " + ((equivalentRoom==null)?"null":equivalentRoom.gameObject.name));
        return equivalentRoom;
    }
}
