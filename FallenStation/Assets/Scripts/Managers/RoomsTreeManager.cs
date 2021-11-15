using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsTreeManager : AbstractSingleton<RoomsTreeManager>
{
    /* Niveau : Arbre de RoomNodes (pi�ces)
    * 
    *           o                            o
    *        /  |  \                      /  |  \ 
    *      o    o   [o] currentRoom     o    o   |o| equivalentRoom
    *     / \       / \                / \       / \
    *    o   o     o   o              o   o     o   o 
    *       / \       /                  / \       / 
    *      o   o     o                  o   o     o  
    *      
    *   Chaque liaison (/) est illustr�e par une porte : relation parent ou enfants
    */

    [SerializeField]
    RoomNode currentRoom = null;
    RoomNode nextRoom = null;
    GameObject lastDoor = null;
    GameObject duplicatedDoor = null;

    bool flagRecreateRootDoor = false;

    public void Start()
    {
        
    }

    public RoomNode GetNextRoom()
    {
        return nextRoom;
    }

    public void ChangeRoom(RoomNode room, GameObject doorToRetrieve = null, bool disableOthers = false)
    {
        nextRoom = room;
        if (doorToRetrieve != null)
        {
            if (duplicatedDoor == doorToRetrieve)
            { /*Hack d'action sur la porte -- cas o� l'on remonte dans la pi�ce*/
                print("duplicatedDoor == doorToRetrieve");
                Door d = lastDoor.GetComponent<Door>();
                Destroy(duplicatedDoor);
                EnableNode(room, false);
                d.optimizeRooms = false;
                d.Open();
                d.optimizeRooms = true;
                currentRoom = room;
                nextRoom = null;

                /* Si la piece parente poss�de une porte � afficher */
                if (currentRoom.GetEntranceDoor() != null)
                {
                    Door entranceDoor = currentRoom.GetEntranceDoor();
                    if (lastDoor != entranceDoor.gameObject)
                    {
                        print("lastDoor != currentRoom.entranceDoor.gameObject");
                        //Copie de l'objet
                        duplicatedDoor = GameObject.Instantiate(entranceDoor.gameObject);
                        print("new door duplicated");
                        //Affectation de position
                        duplicatedDoor.transform.position = entranceDoor.gameObject.transform.position;
                        lastDoor = entranceDoor.gameObject;
                        flagRecreateRootDoor = true;
                        //print("lastDoor equals currentRoom.entranceDoor.gameObject : " + (lastDoor == currentRoom.entranceDoor.gameObject) + " : "+duplicatedDoor.name);
                        return;
                    }
                }
                lastDoor = null;

                return;
            }
        }

        if (duplicatedDoor)
        {
            Destroy(duplicatedDoor);
        }

        
        lastDoor = doorToRetrieve;
        currentRoom = room;
        if (disableOthers)
        {
            UnloadOtherRooms();
        }
        EnableNode(currentRoom, false);
        nextRoom = null;

    }

    public void SwitchTimeline()
    {
        RoomNode newRoom = currentRoom.GetEquivalentRoom();
        GameObject entranceDoor = (newRoom.GetEntranceDoor() == null)? null : newRoom.GetEntranceDoor().gameObject;
        //currentRoom = newRoom;

        lastDoor = entranceDoor;
        flagRecreateRootDoor = false;
        ChangeRoom(newRoom, entranceDoor, true);
    }

    public void UnloadOtherRooms()
    {
        //Si l'on souhaite garder la derniere porte, on en cr�e une copie temporaire dans la sc�ne
        if(flagRecreateRootDoor == false)
        {
            if (lastDoor)
            {
                //Copie de l'objet
                duplicatedDoor = GameObject.Instantiate(lastDoor);
                //Affectation de position
                duplicatedDoor.transform.position = lastDoor.transform.position;
                //On la cache le temps que l'autre soit d�sactiv�e
                duplicatedDoor.SetActive(false);
            }
        }
        print("childrens of " + currentRoom + " : " + currentRoom.GetChildrens().Count);
        if (currentRoom.GetParent())
        {
            DisableNode(currentRoom.GetParent(), true, true);
        }
        foreach (RoomNode child in currentRoom.GetChildrens())
        {
            DisableNode(child, true);
        }
        if (lastDoor)
        {
            //On montre la porte dupliqu�e
            duplicatedDoor.SetActive(true);
        }
        flagRecreateRootDoor = false;
    }

    public RoomNode GetCurrentRoom()
    {
        return currentRoom;
    }

    private void _AffectNode(RoomNode node, bool newState, bool recursive, bool reverseSearch = false)
    {
        if (recursive)
        {
            if (reverseSearch)
            {
                //D�sactivation du noeud
                try
                {
                    RoomNode parent = node.GetParent();
                    if (parent)
                    {
                        _AffectNode(parent, newState, recursive, reverseSearch);
                    }
                }
                catch (System.NullReferenceException _e) { }
            }
            else
            {
                //R�cursivit� sur enfants - effectif uniquement pour childrens.Length > 0 (implicite)
                foreach (RoomNode child in node.GetChildrens())
                {
                    _AffectNode(child, newState, recursive);
                }
            }
        }

        node.SetActive(newState);
    }

    public void EnableNode(RoomNode node, bool recursive, bool reverseSearch = false)
    {
        _AffectNode(node, true, recursive, reverseSearch);
    }
    public void DisableNode(RoomNode node, bool recursive, bool reverseSearch = false)
    {
        Debug.Log("Unloading " + node.name);
        _AffectNode(node, false, recursive, reverseSearch);
    }

}
